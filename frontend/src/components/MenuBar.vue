<template>
  <div class="header-area">
    <div style="width: 48px; margin-left: 8px">
      <div class="logo" />
    </div>
    <div v-if="isMenu" style="background: rgb(60, 60, 60)">
      <my-theme style="height: 100%">
        <hsc-menu-bar style="height: 100%; border-radius: 0 0 4pt 0">
          <hsc-menu-bar-item label="Media" class="menu-item-wrapper">
            <hsc-menu-item label="File">
              <hsc-menu-item label="Save" value="Save" />
              <hsc-menu-item label="Load" value="Load" />
            </hsc-menu-item>
            <hsc-menu-item label="Discovery Camera" />
            <hsc-menu-separator />
            <hsc-menu-item label="Exit" @click="() => {}" />
          </hsc-menu-bar-item>
          <hsc-menu-bar-item label="Manager" class="menu-item-wrapper">
            <hsc-menu-item label="Inference App" @click="createWindow" />
            <hsc-menu-item label="NVR" @click="createWindow" />
          </hsc-menu-bar-item>
          <hsc-menu-bar-item label="View" class="menu-item-wrapper">
            <hsc-menu-item label="Resources">
              <hsc-menu-item
                label="Record Video"
                v-model="selectedViews"
                value="RecordVideo"
              />
              <hsc-menu-item
                label="Rtsp Video"
                v-model="selectedViews"
                value="RtspVideo"
              />
            </hsc-menu-item>

            <hsc-menu-separator />
            <hsc-menu-item
              label="Train Setting"
              v-model="selectedViews"
              value="TrainSetting"
            />
            <hsc-menu-item
              label="Inference Chart"
              v-model="selectedViews"
              value="InferenceChart"
            />
          </hsc-menu-bar-item>
          <hsc-menu-bar-item label="Help" class="menu-item-wrapper">
            <hsc-menu-item label="Welcome">
              <hsc-menu-item label="User"></hsc-menu-item>
              <hsc-menu-item label="Developer"></hsc-menu-item>
            </hsc-menu-item>
            <hsc-menu-item label="Get Started" />
            <hsc-menu-item label="Documentation" />
            <hsc-menu-item label="Release Notes" />
            <hsc-menu-separator />
            <hsc-menu-item label="About" />
          </hsc-menu-bar-item>
        </hsc-menu-bar>
      </my-theme>
    </div>
    <v-spacer style="-webkit-app-region: drag; height: 100%" />

    <div class="action-item-wrapper" @click="handleMinimize">
      <v-icon small>mdi-window-minimize</v-icon>
    </div>
    <div class="action-item-wrapper" @click="handleMaximize">
      <v-icon small>{{
        !maximize ? 'mdi-window-maximize' : 'mdi-window-restore'
      }}</v-icon>
    </div>
    <div class="action-item-wrapper" @click="handleExit">
      <v-icon small>mdi-window-close</v-icon>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { StyleFactory } from '@hscmap/vue-menu';

const active = {
  backgroundColor: '#436f7c',
};
const disabled = {
  opacity: '0.5',
};
const separator = {
  backgroundColor: 'rgba(240, 240, 240, 0.25)',
  height: '.5pt',
  margin: '0pt',
};

interface IMenuProps {
  isMenu: boolean;
}
interface IMenuState {
  maximize: boolean;
  selectedViews: string[];
}

export default Vue.extend<IMenuState, any, any, IMenuProps>({
  name: 'menu-bar',
  props: {
    isMenu: {
      type: Boolean,
      default() {
        return true;
      },
    },
  },
  components: {
    'my-theme': StyleFactory({
      menu: {
        background: 'rgb(60,60,60)',
        color: 'white',
        boxShadow: '0 2pt 4pt rgba(0, 0, 0, 0.5)',
        padding: '20px',
      },
      menubar: {
        height: '100%',
        background:
          'linear-gradient(to bottom, rgba(40,40,40,0), rgba(60,60,60,0))',
        color: 'white',
        boxShadow: '0 4pt 4pt rgba(0, 0, 0, 0)',
      },
      active,
      disabled,
      separator,
      animation: false,
    }),
  },
  data() {
    return {
      maximize: false,
      selectedViews: [],
    };
  },
  methods: {
    createWindow() {
      this.$electron.createWindow('/train');
    },
    handleMinimize() {
      this.$electron.minimize(this.$router.currentRoute.path);
      // window.minimize();
    },
    handleMaximize() {
      // window.maximize();
      this.$electron
        .maximize(this.$router.currentRoute.path)
        .then((result: any) => {
          this.maximize = result;
        });
    },
    handleExit() {
      this.$electron.exit(this.$router.currentRoute.path);
      // window.exit();
    },
  },
});
</script>

<style>
.menu {
  border-radius: 0pt !important;
}
.menu-item-wrapper {
  vertical-align: middle;

  cursor: pointer;
}
.action-item-wrapper {
  margin-left: 0.5em;
  width: 2em;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.action-item-wrapper:hover {
  background: rgba(30, 30, 30, 0.5);
}
.header-area {
  min-height: 30px;
  height: 100%;
  width: 100%;
  background: rgb(60, 60, 60);
  z-index: 10;
  display: flex;
  align-items: center;
}

.header-area > div {
  height: 100% !important;
}

.menuitem {
  cursor: pointer;
  height: 36px !important;
  align-items: center !important;
  justify-content: flex-start;
}
.logo {
  height: 100%;
  margin: 8px;
  background-size: contain !important;
  background-repeat: no-repeat !important;
  background: url('../assets/icn_SaigeVAD.ico');
}
</style>
