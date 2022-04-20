<template>
  <v-app>
    <router-view></router-view>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  data: () => {
    return {};
  },
  created(): void {
    console.log('test');
  },
  mounted(): void {
    console.log('test');
  },
});
</script>

<style lang="scss">
html,
body {
  overflow: hidden !important;
  font-size: 16px;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.v-list {
  padding: 0px !important;
}

.menu-item-wrapper .fixed .menu {
  border-radius: 0pt !important;
  padding: 0pt !important;
}

.menu-item-wrapper .fixed .menu .menuitem {
  padding: 4px 8pt 4px 4pt;
  cursor: pointer;
}
</style>
