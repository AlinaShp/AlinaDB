<template>
  <div id="app">
    <!-- Full-screen blurred background -->
    <div id="blur-background"></div>

    <!-- All pages render here -->
    <router-view />
  </div>
</template>

<script>
export default {
  name: "App",
};
</script>

<style>
/* Make app and html full height */
html, body, #app {
  margin: 0;
  padding: 0;
  height: 100%;
  font-family: "Nunito", Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #f3f3f3;
  background: none; /* Remove default background */
  text-align: center;
  overflow-x: hidden;
}

/* Full-screen blurred background */
#blur-background {
  position: fixed;
  top: 0;
  left: 0;
  width: 110vw;  /* Slightly bigger to avoid edges */
  height: 110vh;
  background-image: url('https://img.freepik.com/free-vector/abstract-geometric-round-shape-blue-background-design_1017-42785.jpg?w=740&t=st=1719245353~exp=1719245953~hmac=94d8da51d60def1686fa7bdec8683557e919fdbf54be7c1402080585482ae44e');
  background-size: cover;
  background-position: center;
  filter: blur(12px);
  z-index: -1;  /* Behind all content */
  transform: scale(1.05);
}

/* Ensure router-view content stacks above background */
router-view {
  position: relative;
  z-index: 1;
}
.swal2-container {
  z-index: 10000 !important;
}
</style>
