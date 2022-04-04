import Vue, { VNode } from "vue";
interface IElectronAPI {
  maximize: () => void;
  minimize: () => void;
  exit: () => void;
}

declare global {
  namespace JSX {
    interface Element extends VNode {}
    interface ElementClass extends Vue {}
    interface IntrinsicElements {
      [elem: string]: any;
    }
  }
}

declare module "vue/types/vue" {
  // 3. Declare augmentation for Vue
  interface Vue {
    $electron: IElectronAPI;
  }
}
