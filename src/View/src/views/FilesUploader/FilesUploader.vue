<template>
  <div class="file__uploader-wrapper">
    <div class="file__uploader-container">
      <FileUpload class="file__uploader-icon"/>

      <p class="file__uploader-text">{{ $t('drag_zone.upload-text') }}</p>

      <p class="file__uploader-or-text">{{ $t('drag_zone.or-text') }}</p>

      <label class="file__uploader-selector">
        {{ $t('drag_zone.select-files-button') }}
        <input type="file" name="files" multiple class="file__uploader-input">
      </label>

      <p class="file__uploader-size">{{ $t('drag_zone.file-size-text') }}</p>
    </div>
  </div>
</template>

<script setup>
import FileUpload from "@/views/FilesUploader/icons/FileUpload.vue";
</script>

<style scoped>
@import "FilesUploader.css";
</style>