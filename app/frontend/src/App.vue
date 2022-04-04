<template>
  <v-app>
    <menu-bar />
    <!-- <router-view></router-view> -->
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import MenuBar from "@/components/MenuBar.vue";

export default Vue.extend({
  components: {
    "menu-bar": MenuBar,
  },
  data: () => {
    return {
      id: "test",
    };
  },
  created(): void {
    console.log("test");
  },
  mounted(): void {
    console.log("test");
  },
});
</script>

<style lang="scss">
html,
body {
  overflow: hidden !important;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
