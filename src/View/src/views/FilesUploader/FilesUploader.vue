<template>
  <div class="file__uploader-wrapper">
    <div class="file__uploader-container">
      <FileUpload class="file__uploader-icon"/>

      <p class="file__uploader-text">{{ $t('drag_zone.upload-text') }}</p>

      <p class="file__uploader-or-text">{{ $t('drag_zone.or-text') }}</p>

      <label class="file__uploader-selector">
        {{ $t('drag_zone.select-files-button') }}
        <input type="file" name="files" multiple class="file__uploader-input" @change="handleFileUpload">
      </label>

      <p class="file__uploader-size">{{ $t('drag_zone.file-size-text') }}</p>
    </div>

    <div v-for="(file, index) in uploadedFiles" :key="index" class="file__uploader-file-info">
      <div class="file__details-container">
        <p class="file__name">{{ formatFileName(file.name) }}</p>
        <p class="file__size">{{ formatFileSize(file.size) }}</p>
      </div>
      <button type="button" @click="removeFile(index)" class="file__remove-btn">
        <font-awesome-icon icon="fa-solid fa-trash-can" />
      </button>
    </div>

    <button
        v-if="uploadedFiles.length"
        class="file__uploader-submit-btn"
        @click="handleSubmit"
    >
      {{ $t('drag_zone.files-upload-submit') }}
    </button>
  </div>
</template>

<script setup>
import FileUpload from "@/views/FilesUploader/icons/FileUpload.vue";
import { ref } from 'vue';
import { useFileUploader } from "@/views/FilesUploader/compositions/useFileUploader.js";

const uploadedFiles = ref([]);

const handleFileUpload = (event) => {
  const files = event.target.files;
  for (let i = 0; i < files.length; i++) {
    uploadedFiles.value.push(files[i]);
  }
};

const handleSubmit = async () => {
  try {
    const path = window.location.pathname;
    const key = path.split('/').pop();

    const { uploadResponse } = await useFileUploader([...uploadedFiles.value], key);

    // Await the _rawValue Promise to get the Blob object
    const blob = await uploadResponse._rawValue;

    const blobUrl = window.URL.createObjectURL(blob);

    const downloadLink = document.createElement('a');
    downloadLink.href = blobUrl;
    downloadLink.download = 'archive.zip';

    document.body.appendChild(downloadLink);
    downloadLink.click();

    document.body.removeChild(downloadLink);
    window.URL.revokeObjectURL(blobUrl);

    uploadedFiles.value = [];
  }
  catch (error) {
    console.error('Upload error:', error);
  }
};

const removeFile = (index) => {
  uploadedFiles.value.splice(index, 1);
};

const formatFileName = (name) => {
  const maxLength = 20;
  if (name.length <= maxLength) return name;
  const firstPart = name.substr(0, maxLength / 2);
  const lastPart = name.substr(name.length - maxLength / 2);
  return `${firstPart}...${lastPart}`;
};

const formatFileSize = (size) => {
  if (size === 0) return '0 Bytes';

  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
  const i = Math.floor(Math.log(size) / Math.log(k));

  return parseFloat((size / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
};
</script>

<style scoped>
@import "FilesUploader.css";
</style>