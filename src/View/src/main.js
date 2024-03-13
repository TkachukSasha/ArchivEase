import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createI18n } from 'vue-i18n'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faUserSecret, faBars, faDownload, faChevronLeft, faChevronRight, faTrashCan, faFileImport  } from '@fortawesome/free-solid-svg-icons';

import App from './App.vue'
import router from './router'
import { languages, defaultLocale } from '@/i18n/index.js';

const messages = { ...languages };

const i18n = createI18n({
    legacy: false,
    locale: defaultLocale,
    messages
});

library.add(faUserSecret, faBars, faDownload, faChevronLeft, faChevronRight, faTrashCan, faFileImport);

const app = createApp(App);

app.use(i18n);
app.use(createPinia());
app.use(router);

app.component('font-awesome-icon', FontAwesomeIcon);

app.mount('#app');