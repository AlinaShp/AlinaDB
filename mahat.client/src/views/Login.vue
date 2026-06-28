<template>
  <div class="login-wrapper">
    <!-- Background Blur Overlay -->
    <div class="bg-blur"></div>

    <!-- Login Container -->
    <div class="login-container" ref="loginBox">
      <h1 class="title">YolinaDB</h1>

      <!-- Instance dropdown -->
      <div class="dropdown-box">
        <label class="label">Select SQL Instance</label>

        <div class="dropdown">
          <button
            class="btn dropdown-toggle instance-btn"
            id="dropdownMenuButton"
            data-bs-toggle="dropdown"
            aria-expanded="false"
          >
            {{ selectedInstance }}
          </button>

          <ul class="dropdown-menu instance-menu" aria-labelledby="dropdownMenuButton">
            <li v-for="instance in availableInstances">
              <a class="dropdown-item"
                 @click="selectInstance(instance)"
                 href="#">
                {{instance}}
              </a>
            </li>
          </ul>
        </div>
      </div>
      <!-- Sign In Button -->
      <button class="pink-btn" @click="signin">Sign In</button>
    </div>
  </div>
</template>
<script>
  import { mapActions } from "vuex";
  import { getUser } from "@/api/UserApi.js";
  import { checkConnection } from "@/api/DBApi"
  import gsap from "gsap";
  import Swal from "sweetalert2";
export default {
  name: "LoginPage",
  data() {
    return {
      selectedInstance: "Select Instance",
      availableInstances: ["WIN-6OA5JFTRURC\\SQLEXPRESS"]
    };
  },
  methods: {
    selectInstance(instance) {
      this.$cookies.set("selectedInstance", instance, "1d");
      this.selectedInstance = instance;
    },
    async signin() {
      try {
        Swal.fire({
          title: "Trying to connect...",
          allowOutsideClick: false,
          allowEscapeKey: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });
        const usernameResponse = await getUser();
        await this.$store.dispatch("changeUser", usernameResponse.data);

        const response = await checkConnection(this.selectedInstance)

        Swal.fire({
          icon: "success",
          title: "Connected",
          text: response.data,
          timer: 1500,
          showConfirmButton: false,
        });

        this.$router.push("/home");

      } catch (err) {
        const msg = err.response?.data?.message || err.message;
        Swal.fire({
          icon: "error",
          title: "Failed to connect to instance",
          text: msg,
        });
      }
    },
  },
  mounted() {
    // Animated fade + slide for login window
    gsap.from(this.$refs.loginBox, {
      duration: 1.2,
      opacity: 0,
      y: 40,
      ease: "power3.out",
    });
  },
};
</script>
<style scoped>
/* Fullscreen wrapper */
.login-wrapper {
  position: relative;
  width: 100%;
  height: 100vh;
  background-image: url("/bg.jpg");
  background-size: cover;
  background-position: center;
  overflow: hidden;
}
/* Background blur overlay */
.bg-blur {
  position: absolute;
  inset: 0;
  backdrop-filter: blur(18px) brightness(0.6);
}
/* Login card */
.login-container {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 520px;
  padding: 40px 50px;
  background: rgba(30, 40, 70, 0.6);
  border-radius: 16px;
  border: 1px solid rgba(255, 105, 180, 0.4);
  box-shadow: 0 0 25px rgba(255, 20, 147, 0.4);
  backdrop-filter: blur(12px);
  text-align: center;
}
/* Title */
.title {
  font-family: "Rubik", sans-serif;
  font-weight: 700;
  font-size: clamp(2.8rem, 6vw, 6.5rem); /* responsive instead of fixed 7em */
  color: pink;
  margin: 0 auto 25px auto; /* ensures perfect horizontal centering */
  text-align: center;
  width: 100%;
  display: block;
  text-shadow: 0 0 18px rgba(255, 140, 200, 0.6);
}
/* Dropdown container */
.dropdown-box {
  margin-bottom: 30px;
  text-align: left;
}
.label {
  color: pink;
  font-size: 1.3em;
  margin-bottom: 6px;
  display: block;
}
/* Instance dropdown button */
.instance-btn {
  width: 100%;
  background-color: #2e3a62 !important;
  border: 1px solid #ff55c5;
  color: #ffe6fb !important;
  font-size: 1.15em;
  padding: 10px;
  text-align: left;
  border-radius: 6px;
  transition: 0.2s ease;
}
.instance-btn:hover {
  background-color: #3f4c7f !important;
  border-color: #ff8ddd;
}
/* Dropdown menu styling */
.instance-menu {
  background: #2e3a62;
  border: 1px solid #ff55c5;
}
.dropdown-item {
  color: #ffe6fb;
}
.dropdown-item:hover {
  background: #ff80d0;
  color: white;
}
/* Sign in button */
.pink-btn {
  width: 100%;
  padding: 14px;
  margin-top: 10px;
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  border: none;
  border-radius: 6px;
  font-size: 1.3em;
  color: #2b1f29;
  font-weight: bold;
  cursor: pointer;
  transition: 0.25s;
  box-shadow: 0 0 12px rgba(255, 100, 180, 0.5);
}
.pink-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 18px rgba(255, 120, 200, 0.8);
}
</style>
