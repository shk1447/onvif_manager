<template>
  <v-app id="app">
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
  height: 100%;
}

::-webkit-scrollbar {
  width: 0.5rem;
  height: 0.5rem;
}

::-webkit-scrollbar-corner {
}

::-webkit-scrollbar-thumb {
  transition: 0.3s ease all;
  border-color: transparent;
  background-color: rgba(151, 151, 151, 0.12);
  z-index: 40;
}

#app {
  height: 100%;
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
