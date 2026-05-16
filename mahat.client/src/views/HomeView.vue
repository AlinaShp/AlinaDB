<template>
  <div>
    <DBview></DBview>
  </div>
</template>

<script>
import DBview from "./DBview.vue";
import { mapActions, mapGetters, mapState, mapMutations } from "vuex";

export default {
  name: "HomeView",
  components: {
    DBview,
  },

  computed: {
    user() {
      return this.$store.state.user;
    },
    ...mapState(["themeMode"]),
  },
};
</script>
