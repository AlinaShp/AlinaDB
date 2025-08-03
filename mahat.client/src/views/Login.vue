<template>
    <div >

       <div v-if="!isAuthenticated">
        <transition
        appear
        @before-enter="beforeEnter"
        @enter="enter"
        >
          <h1>AlinaDB</h1>
        </transition>
        <h2 class="wordchanger">
        </h2>
        <button type="button" class="btn btn-light" @click="signin">Sign In</button>
      </div>
      <div class="dropdown">
                  <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                    {{ selectedInstance }}
                  </button>
                  <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                    <li><a class="dropdown-item"  @click="selectInstance('WIN-6OA5JFTRURC\\SQLEXPRESS')" href="#">WIN-6OA5JFTRURC\SQLEXPRESS</a></li>
                  </ul>
                </div>
    </div>
  </template>
  
  <script>
  // @ is an alias to /src
  import LoginPage from '@/components/LoginPage.vue';
  import newDB from "@/components/newDB.vue";
  import gsap from 'gsap';
  import { mapActions, mapGetters } from 'vuex';
  import { getUser } from '@/api/UserApi.js'
  
  export default {
    name: 'Login',
    components: {
      LoginPage,
      newDB
    },
   
    setup(){
      const beforeEnter=(el) =>{
        console.log("beforeenter");
        el.style.transform = 'translateY(-120px)'
        el.style.opacity = 0
      }

      const enter = (el) => {
        console.log("starting to enter");
        gsap.to(el, {
          y:0,
          opacity: 1,
          duration: 2,
          ease: 'bounce.out'
        })

      }
      return {beforeEnter, enter}
    },
    data() {
      return {
        selectedInstance: 'select instance'
      }
    },
    computed: {
      user() {
        return this.$store.state.user;
      }
    },
    methods: {
      selectInstance(instance) {
        this.$cookies.set('selectedInstance', instance, '1d')
        this.selectedInstance = instance;
    },
    async signin(){
      try {
        const usernameResponse = await getUser();
        await this.$store.dispatch('changeUser', usernameResponse.data);
        
        this.$router.push('/home')
      } catch (error) {
        console.error('Error logging in:', error);
      }
    }

    },
    
    mounted(){
      localStorage.clear();
    }
  }
  </script>
  <style scoped>
@import url('https://fonts.googleapis.com/css2?family=Jolly+Lodger&family=Oswald:wght@200..700&family=Rubik:ital,wght@0,300..900;1,300..900&display=swap');  h1{
    font-family: "Rubik", system-ui;
  font-weight: 700;
    font-size: 15em;
   color: pink;
  }
  h2{
    font-size: 4em;
    color: pink;
  }
  .wordchanger:before{
    content: 'Alina';
    animation-name: mywordchange;
    animation-duration: 6s;
    animation-iteration-count: infinite;
    animation-timing-function: ease;
  }
  @keyframes mywordchange {
    0%{content: 'Alina'; opacity: 0.5;}
    5%{content: 'Alina'; opacity: 1;}
    10%{content: 'Alina'; opacity: 0;}

    15%{content: 'SH port'; opacity: 0;}
    20%{content: 'SH port';opacity: 1;}
    25%{content: 'SH port';opacity: 0;}

    30%{content: 'Presents';opacity: 0;}
    35%{content: 'Presents';opacity: 1;}
    40%{content: 'Presents';opacity: 0;}

    45%{content: 'AlinaDB';opacity: 0;}
    50%{content: 'AlinaDB';opacity: 1;}
    55%{content: 'AlinaDB';opacity: 0;}

    60%{content: 'Application';opacity: 0;}
    65%{content: 'Application';opacity: 1;}
    70%{content: 'Application';opacity: 0;}

    75%{content: 'Sign in to start';opacity: 0;}
    80%{content: 'Sign in to start';opacity: 1;}
    85%{content: 'Sign in to start';opacity: 0;}


  }
</style>
