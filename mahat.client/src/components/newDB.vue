<template>
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Document</title>
  </head>
  <div id="dbComponent">
    <div class="row">
    <div class="custom-sidebar col-1">

      <div class="sidebar-btn">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke-width="1.5"
          stroke="currentColor"
          class="w-6 h-6"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            d="M8.25 4.5l7.5 7.5-7.5 7.5"
          />
        </svg>
      </div>
      
      <div class="logo">ADB</div>

      <div class="menu">
        <div class="menu-title">Menu</div>

        <a href="#" @click="dbButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              class="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M2.25 12l8.954-8.955c.44-.439 1.152-.439 1.591 0L21.75 12M4.5 9.75v10.125c0 .621.504 1.125 1.125 1.125H9.75v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21h4.125c.621 0 1.125-.504 1.125-1.125V9.75M8.25 21h8.25"
              />
            </svg>
          </div>
          <div class="label">Databases</div>
        </a>

        <a href="#" @click="customQueryButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              class="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M9.594 3.94c.09-.542.56-.94 1.11-.94h2.593c.55 0 1.02.398 1.11.94l.213 1.281c.063.374.313.686.645.87.074.04.147.083.22.127.324.196.72.257 1.075.124l1.217-.456a1.125 1.125 0 011.37.49l1.296 2.247a1.125 1.125 0 01-.26 1.431l-1.003.827c-.293.24-.438.613-.431.992a6.759 6.759 0 010 .255c-.007.378.138.75.43.99l1.005.828c.424.35.534.954.26 1.43l-1.298 2.247a1.125 1.125 0 01-1.369.491l-1.217-.456c-.355-.133-.75-.072-1.076.124a6.57 6.57 0 01-.22.128c-.331.183-.581.495-.644.869l-.213 1.28c-.09.543-.56.941-1.11.941h-2.594c-.55 0-1.02-.398-1.11-.94l-.213-1.281c-.062-.374-.312-.686-.644-.87a6.52 6.52 0 01-.22-.127c-.325-.196-.72-.257-1.076-.124l-1.217.456a1.125 1.125 0 01-1.369-.49l-1.297-2.247a1.125 1.125 0 01.26-1.431l1.004-.827c.292-.24.437-.613.43-.992a6.932 6.932 0 010-.255c.007-.378-.138-.75-.43-.99l-1.004-.828a1.125 1.125 0 01-.26-1.43l1.297-2.247a1.125 1.125 0 011.37-.491l1.216.456c.356.133.751.072 1.076-.124.072-.044.146-.087.22-.128.332-.183.582-.495.644-.869l.214-1.281z"
              />
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
              />
            </svg>
          </div>
          <div class="label">Custom Query</div>
        </a>

        <a href="#">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              class="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M4.26 10.147a60.436 60.436 0 00-.491 6.347A48.627 48.627 0 0112 20.904a48.627 48.627 0 018.232-4.41 60.46 60.46 0 00-.491-6.347m-15.482 0a50.57 50.57 0 00-2.658-.813A59.905 59.905 0 0112 3.493a59.902 59.902 0 0110.399 5.84c-.896.248-1.783.52-2.658.814m-15.482 0A50.697 50.697 0 0112 13.489a50.702 50.702 0 017.74-3.342M6.75 15a.75.75 0 100-1.5.75.75 0 000 1.5zm0 0v-3.675A55.378 55.378 0 0112 8.443m-7.007 11.55A5.981 5.981 0 006.75 15.75v-1.5"
              />
            </svg>
          </div>
          <div class="label">Courses</div>
        </a>

        <a href="#">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              class="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M3 16.5v2.25A2.25 2.25 0 005.25 21h13.5A2.25 2.25 0 0021 18.75V16.5M16.5 12L12 16.5m0 0L7.5 12m4.5 4.5V3"
              />
            </svg>
          </div>
          <div class="label">Downloads</div>
        </a>
      </div>

      <div class="menu">
        <div class="menu-title">Links</div>
        <a href="#">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              class="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M14.857 17.082a23.848 23.848 0 005.454-1.31A8.967 8.967 0 0118 9.75v-.7V9A6 6 0 006 9v.75a8.967 8.967 0 01-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 01-5.714 0m5.714 0a3 3 0 11-5.714 0"
              />
            </svg>
          </div>
          <div class="label">Notifications</div>
        </a>

        <a href="#">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              class="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M3 13.125C3 12.504 3.504 12 4.125 12h2.25c.621 0 1.125.504 1.125 1.125v6.75C7.5 20.496 6.996 21 6.375 21h-2.25A1.125 1.125 0 013 19.875v-6.75zM9.75 8.625c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125v11.25c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 01-1.125-1.125V8.625zM16.5 4.125c0-.621.504-1.125 1.125-1.125h2.25C20.496 3 21 3.504 21 4.125v15.75c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 01-1.125-1.125V4.125z"
              />
            </svg>
          </div>
          <div class="label">Statistics</div>
        </a>
      </div>
    </div>
    <QueryModal ref="modalComponent" />

    <div v-if="dbCardIsVisible" id="cards">
        <div  class="row">
          <div class="col-4" v-for="card in cards" :key="card.id">
            <DBcard :card="card" @buttonClick="tableButtonClick"></DBcard>
          </div>
        </div>
        
      </div>
      <div v-if="tableIsVisible" class="col-10" id="table">
          <Table></Table>
        </div>
      </div>
    </div>


  

  
</template>

<script>
import DBcard from "./DBcard.vue";
import Table from "../views/Table.vue";
import QueryModal from "./QueryModal.vue";
import { getDBinfo } from '@/api/DBApi';
export default {
    data(){
        return{
            cards:[],
            dbCardIsVisible: true,
            tableIsVisible:false
        };
    },
    components: {
        DBcard,
        Table,
        QueryModal
    },
    methods: {
    dbButtonClick() {

    },
    tableButtonClick() {
      this.dbCardIsVisible=false;
      this.tableIsVisible=true;
    },
    customQueryButtonClick(){
      console.log("triggered");
      this.$refs.modalComponent.showModal();
    },
    async getDBinfo(){

    try {
      const instanceName = this.$globals.instanceName;
        const response = await getDBinfo(instanceName);
        console.log(response.data);
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
<style scoped>

body{
   background-image: url('https://img.freepik.com/free-vector/abstract-geometric-round-shape-blue-background-design_1017-42785.jpg?w=740&t=st=1719245353~exp=1719245953~hmac=94d8da51d60def1686fa7bdec8683557e919fdbf54be7c1402080585482ae44e') !important; 
  background-size: cover;
  backdrop-filter: blur(20px);
  position: relative

}
#table{
  margin-left: 20vh;
}
#cards{
    margin-left: 20vh;
    width: 120vh;
    z-index: 0;
}

.col-6{
    width: 37%;
}
.custom-sidebar .menu svg {
  height: 30px;
  color: pink;
}

.custom-sidebar {
  z-index: 1;
  position: fixed;
  background: rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
  font-family: "Roboto", sans-serif;
  width: 80px;
  top: 24px;
  left: 24px;
  border-radius: 10px;
  box-shadow: 0 8px 32px rgba(140, 42, 185, 0.4);
  overflow: hidden;
  color: #2a1e0d;
  transition: all 400ms cubic-bezier(0.07, 0.88, 0.37, 1.05);
  width: 10vh;
  height: 80vh;
}

.custom-sidebar .label {
  opacity: 0;
  pointer-events: none;
  transform: translateX(-20px);
  transition: all 600ms ease;
}

.custom-sidebar:hover .label {
  opacity: 1;
  pointer-events: auto;
  transform: translateX(0);
}

.custom-sidebar:hover {
  width: 300px;
}

.custom-sidebar:hover .menu a {
  margin: 0 24px;
}

.custom-sidebar:hover .menu-title {
  padding: 0 32px;
}

.custom-sidebar .menu a {
  color: pink;
}

.custom-sidebar .logo {
  font-size: 20px;
  font-weight: 900;
  
  color: hotpink;
}

.custom-sidebar .menu {
  border-top: 1px solid rgba(13, 27, 42, 0.3);
  padding: 16px 0;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.custom-sidebar .menu-title {
  font-size: 14px;
  color: pink;
  text-transform: uppercase;
  margin: 8px 0;
  transition: all 400ms ease;
}

.custom-sidebar .menu a {
  display: flex;
  gap: 24px;
  align-items: center;
  text-decoration: none;
  padding: 1.5vh;
  border-radius: 10px;
  transition: all 500ms ease;
}

.custom-sidebar .menu a:hover {
  background: rgba(255, 255, 255, 0.2);
}

.custom-sidebar .sidebar-btn {
  position: absolute;
  width: 26px;
  top: 50%;
  transform: translateY(-50%);
  right: -26px;
  background: #0d1b2a;
  color: #fff;
  display: flex;
  padding: 8px 0;
  border-radius: 0 4px 4px 0;
  cursor: pointer;
}

@media (max-width: 600px) {
  .custom-sidebar {
    overflow: visible;
    width: 300px;
    transform: translateX(-100%);
    left: 0;
  }

  .custom-sidebar.active {
    transform: translateX(0);
  }

  .custom-sidebar .label {
    opacity: 1;
    transform: translateX(0);
  }

  .custom-sidebar .logo {
    border-radius: 10px 10px 0 0;
  }

  .custom-sidebar:hover .menu-title {
    padding: 0 20px;
  }

  .custom-sidebar .menu a {
    margin: 0 24px;
  }
}
</style>