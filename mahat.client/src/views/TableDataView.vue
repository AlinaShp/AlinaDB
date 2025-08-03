<template>
  <Navbar></Navbar>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import { tablesInfo } from "@/api/DBApi";
export default {
  components: {
    TableCard,
    Navbar,
  },
  props: {
    dbName: {
      type: String,
      required: true,
    },
  },

  data() {
    return {
      tableCards: [],
    };
  },
  methods: {
    async tablesInfo() {
      try {
        const instanceName = this.$cookies.get('selectedInstance')
        const response = await tablesInfo(this.dbName, instanceName);
        const res = response.data;
        console.log(res);
        this.tableCards = [...res];
      } catch (error) {
        console.error("There was an error!", error);
      }
    },
  },
  mounted() {
    this.tablesInfo();
  },
};
</script>

<style scoped></style>
