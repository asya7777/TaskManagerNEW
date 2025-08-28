import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './assets/sass/style.scss'

import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

const app = createApp(App)

app.use(router)

app.use(Toast, {
    position: "top-right",
    timeout: 3000,
    closeOnClick: true,
    pauseOnHover: true
});

app.mount('#app')

