<template>
  <div class="home">
    <menu-bar />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import MenuBar from "@/components/MenuBar.vue";

export default Vue.extend({
  name: "home-view",
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
