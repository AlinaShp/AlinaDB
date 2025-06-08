<template>
    <body>
       <DBview></DBview> 
    </body>
  </template>
  
  <script>
  import LoginPage from '@/components/LoginPage.vue';
  import DBview from "./DBview.vue";
  import { mapActions, mapGetters,  mapState, mapMutations } from 'vuex';
  
  export default {
    name: 'HomeView',
    components: {
      LoginPage,
      DBview
    },
    
    computed: {
      ...mapGetters(['isAuthenticated']),
      user() {
        return this.$store.state.user;
      },
      ...mapState(['themeMode']),
    },
  }
  </script>
  
  