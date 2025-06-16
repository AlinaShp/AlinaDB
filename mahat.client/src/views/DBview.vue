<template>
    <body>
      
    <ColorModeSwitch></ColorModeSwitch>
  <Navbar></Navbar>
  <div id="cards">
          <div  class="row">
            <div class="col-4" v-for="card in cards" :key="card.id">
              <DBcard :card="card" @buttonClick="tableButtonClick(card.DatabaseName)"></DBcard>
            </div>
          </div>   
  </div>
    </body>
  
  </template>
  <script>
  import Navbar from '../components/Navbar.vue';
  import DBcard from "../components/DBcard.vue";
  import QueryModal from "../components/QueryModal.vue";
  import ColorModeSwitch from "../components/ColorModeSwitch.vue";
  import { getDBinfo } from '@/api/DBApi';
  export default {
      components: {
      Navbar,
      DBcard,
      QueryModal,
      ColorModeSwitch
    },
    
      data(){
          return{
              cards:[],
  
          };
      },
      methods: {
  
      tableButtonClick(dbName) {
          this.$router.push({ path: '/DBview/TablesView', query: { dbName: dbName } });
      },
      customQueryButtonClick(){
        this.$refs.modalComponent.showModal();
  
      },
      async getDBinfo(){
      try {
        const instanceName = this.$cookies.get('selectedInstance')
          const response = await getDBinfo(instanceName);
          this.cards = response.data;
      } catch (error) {
          console.error('There was an error!', error);
      }
        
      
    }
   },mounted(){
       this.getDBinfo();
      
    }
  
  }
  </script>
  <style>
  #cards{
      margin-left: 15vh;
      width: 130vh;
      z-index: 0;
  }
  body{
     background-image: url('https://img.freepik.com/free-vector/abstract-geometric-round-shape-blue-background-design_1017-42785.jpg?w=740&t=st=1719245353~exp=1719245953~hmac=94d8da51d60def1686fa7bdec8683557e919fdbf54be7c1402080585482ae44e') !important; 
    background-size: cover;
    backdrop-filter: blur(20px);
    position: relative
  
  }
  
  .modal-backdrop { 
     display: none !important;
  }
  
  
  </style>
