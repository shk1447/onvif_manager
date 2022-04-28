<template>
  <div class="grid-container">
    <div class="grid-header">
      <menu-bar :isMenu="true" />
    </div>
    <div class="grid-panel">
      <left-panel />
    </div>
    <div class="grid-content">
      <content-layout />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import MenuBar from '../components/MenuBar.vue';
import LeftPanel from '../components/LeftPanel.vue';
import ContentLayout from '../components/ContentLayout.vue';

export default Vue.extend<any, any, any, any>({
  components: {
    'menu-bar': MenuBar,
    'left-panel': LeftPanel,
    'content-layout': ContentLayout,
  },
  data() {
    return {};
  },
});
</script>

<style lang="scss" scoped>
.grid-container {
  height: 100%;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(1%, auto));
  grid-template-rows: repeat(auto-fill, minmax(1%, auto));
}
.grid-header {
  z-index: 999999999;
  grid-column: 1/ 101;
  grid-row: 1/6;
}

.grid-panel {
  margin-top: 4px;
  margin-left: 4px;
  grid-column: 1/ 21;
  grid-row: 6/101;
}

.grid-content {
  margin: 4px;
  grid-column: 21/ 101;
  grid-row: 6/101;
}
</style>
