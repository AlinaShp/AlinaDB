import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store/idex.js'
import "bootstrap/dist/css/bootstrap.css"
import "bootstrap/dist/js/bootstrap.bundle.min.js"
import globals from '../globas.js';
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';

const app = createApp(App);
app.config.globalProperties.$globals = globals;

app.use(router);
app.use(store);
app.mount('#app');