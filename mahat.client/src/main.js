import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store/index.js'
import "bootstrap/dist/css/bootstrap.css"
import "bootstrap/dist/js/bootstrap.bundle.min.js"
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import VueCookies from 'vue-cookies';

const app = createApp(App);

app.use(router);
app.use(store);
app.use(VueCookies);
app.mount('#app');
