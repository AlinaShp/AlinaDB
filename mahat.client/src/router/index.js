import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue';
import Login from "../views/Login.vue";
import DBview from '../views/DBview.vue';
import TablesView from '../views/TablesView.vue';
import Table from "../views/Table.vue";


import { useStore } from 'vuex'

const routes = [

  // { path: '/', component: HomeView, meta: { requiresAuth: true } },
  { path: '/', component: Login, meta: { requiresAuth: true } },
  { path: '/home', component: HomeView },
  { path: '/DBview', component: DBview },
  
  { path: '/DBview/TablesView',
    component: TablesView,
    props: (route) => ({ dbName: route.query.dbName }),
  },
  { path: '/DBview/TablesView/TableData',
    component: Table,
    props: (route) => ({ tableName: route.query.tableName, databaseName: route.query.databaseName }),
  },



]

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router
