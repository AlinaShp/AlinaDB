<template>     
    <!-- <div class="home" id="home">
      <div :class="themeMode">
      <button @click="setThemeMode('light')">Light Mode</button>
      <button @click="setThemeMode('dark')">Dark Mode</button>
      <button @click="setThemeMode('custom')">Custom Mode</button>
      <p>This is a sample text to demonstrate different modes.</p>
    </div>
      
  
    </div> -->
  </template>

<script>
import { mapActions, mapGetters,  mapState, mapMutations } from 'vuex';

export default {
  computed: {
    ...mapState(['themeMode']),
  },
  methods: {
    ...mapMutations(['setThemeMode']), 
  },

}
</script>

<style >
/* body {
  font-family: Arial, sans-serif;
  transition: background-color 0.3s, color 0.3s;
}

body.light {
  background-color: white;
  color: black;
}

body.dark {
  background-color: black;
  color: white;
}

body.custom {
  background-color: #f0e68c; 
  color: #4b0082;        
} */
</style>