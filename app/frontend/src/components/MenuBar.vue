<template>
  <div class="header-area">
    <div v-if="isMenu" style="background: rgb(60, 60, 60)">
      <my-theme style="height: 100%">
        <hsc-menu-bar style="height: 100%; border-radius: 0 0 4pt 0">
          <hsc-menu-bar-item label="Media" class="menu-item-wrapper">
            <hsc-menu-item label="File" @click="() => {}" />
            <hsc-menu-item label="Network">
              <hsc-menu-item label="Discovery Onvif" />
              <hsc-menu-item label="Input URL" />
            </hsc-menu-item>
            <hsc-menu-separator />
            <hsc-menu-item label="Exit" @click="() => {}" />
          </hsc-menu-bar-item>
          <hsc-menu-bar-item label="Tool" class="menu-item-wrapper">
            <hsc-menu-item label="Anomaly Detect" @click="() => {}" />
            <hsc-menu-item label="Model">
              <hsc-menu-item label="Model" @click="createWindow" />
            </hsc-menu-item>
          </hsc-menu-bar-item>
          <hsc-menu-bar-item label="View" class="menu-item-wrapper">
            <hsc-menu-item label="Media List">
              <hsc-menu-item
                label="Record"
                v-model="selectedViews"
                value="Record"
              />
              <hsc-menu-item
                label="Realtime"
                v-model="selectedViews"
                value="Realtime"
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

    <div class="action-item-wrapper" @click="minimize">
      <v-icon small>mdi-window-minimize</v-icon>
    </div>
    <div class="action-item-wrapper" @click="maximize">
      <v-icon small>mdi-window-maximize</v-icon>
    </div>
    <div class="action-item-wrapper" @click="exit">
      <v-icon small>mdi-window-close</v-icon>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { StyleFactory } from "@hscmap/vue-menu";

const active = {
  backgroundColor: "#436f7c",
};
const disabled = {
  opacity: "0.5",
};
const separator = {
  backgroundColor: "rgba(240, 240, 240, 0.25)",
  height: ".5pt",
  margin: "0pt",
};

export default Vue.extend({
  name: "menu-bar",
  props: {
    isMenu: {
      type: Boolean,
      default() {
        return true;
      },
    },
  },
  components: {
    "my-theme": StyleFactory({
      menu: {
        background: "rgb(60,60,60)",
        color: "white",
        boxShadow: "0 2pt 4pt rgba(0, 0, 0, 0.5)",
        padding: "20px",
      },
      menubar: {
        height: "100%",
        background:
          "linear-gradient(to bottom, rgba(40,40,40,0), rgba(60,60,60,0))",
        color: "white",
        boxShadow: "0 4pt 4pt rgba(0, 0, 0, 0)",
      },
      active,
      disabled,
      separator,
      animation: false,
    }),
  },
  data() {
    return {
      selectedViews: [],
    };
  },
  methods: {
    createWindow() {
      this.$electron.createWindow("/sub");
    },
    minimize() {
      this.$electron.minimize();
      // window.minimize();
    },
    maximize() {
      // window.maximize();
      this.$electron.maximize();
    },
    exit() {
      this.$electron.exit();
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
  padding: 0em 1em 0em 1em !important;
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
</style>
