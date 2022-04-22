<template>
  <div style="width: 100%; height: 100%" ref="layout" v-resize="onResize"></div>
</template>

<script lang="ts">
import RtspVideo from './contents/RtspVideo.vue';
import EventBus from '../EventBus';

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
  created() {
    EventBus.$on('addContent', (payload: any) => {
      this.gl.addComponent('example', payload, payload.name);
    });
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
        var content = new RtspVideo({
          data: state,
        }).$mount(container.element);
        (container as any)['_content'] = content;
      },
    );
    gl.loadLayout({
      root: {
        type: 'stack',
        content: [] as ComponentItemConfig[],
      },
    } as LayoutConfig);

    gl.on('stateChanged', () => {
      console.log('state changed');
    });
    gl.on('beforeItemDestroyed', (item: any) => {
      if (item.origin._container) {
        item.origin._container._content.destroyPlayer();
        setTimeout(() => {
          item.origin._container._content.$destroy();
        }, 0);
      }
    });

    this.gl = gl;
    // gl.addComponent('example', {}, 'test');
    // gl.addComponent('example', {}, 'test2');
    // gl.addComponent('example', {}, 'test3');
    // gl.addComponent('example', {}, 'test4');
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
