<template>

  <section class="vh-100" style="background-color: #0099C2;">
  <div class="container py-5 h-100">
    <div class="row d-flex justify-content-center align-items-center h-100">
      <div class="col col-xl-10">
        <div class="card" style="border-radius: 1rem;">
          <div class="row g-0">
            <div class="col-md-6 col-lg-5 d-none d-md-block">
              <img src="https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEiWySA41OUwW4l6rmnHkRc0GpW9bLzLmV6Ra3q9x3p3mBgjDLAzLqIFHzJs94fmDiHnX1gdjNRE_m0A7tBoUv2oQ15mLjRcuDpiyZedi6sjY8x-Qj1tfDpzFtOGw0HGbM69aRFMl7pMBEWB/s624/AzureSQLDBUser00.png"
                alt="login form" class="img-fluid" style="border-radius: 1rem 0 0 1rem;" id="loginImg" />
            </div>
            <div class="col-md-6 col-lg-7 d-flex align-items-center">
              <div class="card-body p-4 p-lg-5 text-black">

                <form>

                  <div class="d-flex align-items-center mb-3 pb-1">
                    <i class="fas fa-cubes fa-2x me-3" style="color: #ff6219;"></i>
                    <span class="h1 fw-bold mb-0">Login into your domain account</span>
                  </div>

                  <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Or you won't be able to use this beautiful app</h5>

                  <div data-mdb-input-init class="form-outline mb-4">
                    <input type="email" id="form2Example17" class="form-control form-control-lg" />
                    <label class="form-label" for="form2Example17">Username</label>
                  </div>

                  <div data-mdb-input-init class="form-outline mb-4">
                    <input type="password" id="form2Example27" class="form-control form-control-lg" />
                    <label class="form-label" for="form2Example27">Password</label>
                  </div>
                  <div class="dropdown">
  <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    choose the server
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item" href="#">SQL1</a></li>
    <li><a class="dropdown-item" href="#">SQL2</a></li>
    <li><a class="dropdown-item" href="#">SQL3</a></li>
  </ul>
</div>

                  <div class="pt-1 mb-4">
                    <button data-mdb-button-init data-mdb-ripple-init class="btn btn-dark btn-lg btn-block" type="button" @click="login">Login</button>
                  </div>

                </form>

              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>



</template>

<script>
import axios from 'axios';
import { getUser } from '@/api/UserApi.js'
import { getDBinfo } from '@/api/DBApi.js'

export default {
  name: 'LoginPage',
  props: {
    msg: String
  },
  data() {
    return {};
  },
  methods: {
    async login() {
      console.log("en")
      try {
        const usernameResponse = await getUser();
        await store.dispatch('changeUser', usernameResponse.data);
        const instanceName = this.$globals.instanceName;
        const response = await getDBinfo(instanceName);
        console.log(response.data);
        this.$router.push('/home')
      } catch (error) {
        console.error('Error logging in:', error);
      }
  },mounted(){
    // this.getData();
    
  }
}}
</script>

<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
#loginImg{
  margin: 50px;
}
</style>