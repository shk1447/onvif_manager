<template>
  <div style="width: 100%; height: 100%" ref="layout" v-resize="onResize"></div>
</template>

<script lang="ts">
import RtspVideo from './contents/RtspVideo.vue';

import {
  ComponentContainer,
  ComponentItemConfig,
  ContentItem,
  GoldenLayout,
  JsonValue,
  LayoutConfig,
} from 'golden-layout';

import Vue from 'vue';
interface IContentLayout {
  gl: GoldenLayout;
}
export default Vue.extend<any, any, any, IContentLayout>({
  data() {
    return {};
  },
  methods: {
    onResize() {
      this.gl && this.gl.updateSize(10, 10);
      this.gl && this.gl.updateSize();
    },
  },
  mounted() {
    var gl: GoldenLayout = new GoldenLayout(this.$refs.layout as HTMLElement);
    gl.registerComponentFactoryFunction(
      'example',
      (
        container: ComponentContainer,
        state: JsonValue | undefined,
        virtual: boolean,
      ) => {
        var aa = new RtspVideo({
          data: {
            url: 'https://www.w3schools.com/html/mov_bbb.mp4',
          },
        }).$mount(container.element);
      },
    );
    gl.loadLayout({
      root: {
        type: 'stack',
        content: [] as ComponentItemConfig[],
      },
    } as LayoutConfig);

    this.gl = gl;
    gl.addComponent('example', {}, 'test');
    gl.addComponent('example', {}, 'test2');
    gl.addComponent('example', {}, 'test3');
    gl.addComponent('example', {}, 'test4');
  },
});
</script>

<style>
@import 'golden-layout/dist/css/goldenlayout-base.css';
@import 'golden-layout/dist/css/themes/goldenlayout-borderless-dark-theme.css';
.lm_header .lm_tab.lm_active.lm_focused {
  background: #222222 !important;
}
.lm_root {
  position: absolute !important;
}
.lm_splitter {
  border: 1px solid #222222;
}
</style>
