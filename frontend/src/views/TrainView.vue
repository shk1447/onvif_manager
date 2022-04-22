<template>
  <div class="grid-container">
    <div class="grid-header">
      <menu-bar :isMenu="false" />
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
        Inference App Manager!!!!!!
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import MenuBar from '../components/MenuBar.vue';

export default Vue.extend({
  components: {
    'menu-bar': MenuBar,
  },
  data: () => {
    return {
      id: 'test',
    };
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
  margin: 4px;
  grid-column: 1/ 101;
  grid-row: 5/101;
}
</style>
