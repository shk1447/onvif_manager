<template>
  <div class="header-area">
    <div style="background: rgb(60, 60, 60); height: 32px">
      <my-theme style="height: 100%">
        <hsc-menu-bar style="height: 100%; border-radius: 0 0 4pt 0">
          <hsc-menu-bar-item label="File" class="menu-item-wrapper">
          </hsc-menu-bar-item>
          <hsc-menu-bar-item label="Group" class="menu-item-wrapper">
            <hsc-menu-item label="Search" @click="() => {}" />
            <hsc-menu-separator />
            <hsc-menu-item label="Manage" @click="() => {}" />
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
  methods: {
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

<style scoped>
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
  height: 36px;
  width: 100%;
  background: rgb(60, 60, 60);
  z-index: 10;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
