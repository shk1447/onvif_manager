<template>
  <div class="grid-container">
    <div class="grid-header">
      <menu-bar :isMenu="false" title="Inference App Manager" />
    </div>
    <div class="grid-body">
      <div
        style="
          height: 100%;
          width: 100%;
          display: flex;
          justify-content: center;
          align-items: center;
        "
      >
        <inference-process />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import MenuBar from '../components/MenuBar.vue';
import InferenceProcess from '../components/contents/InferenceProcess.vue';

export default Vue.extend({
  components: {
    'menu-bar': MenuBar,
    'inference-process': InferenceProcess,
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
  grid-row: 1/5;
}

.grid-body {
  grid-column: 1/ 101;
  grid-row: 5/101;
}
</style>
