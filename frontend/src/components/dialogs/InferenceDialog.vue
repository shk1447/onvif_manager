<template>
  <v-dialog v-model="showDialog" max-width="500px" :persistent="true">
    <v-card tile outlined ref="ref_new_inference">
      <v-container>
        <v-form>
          <v-row>
            <v-col>
              <v-select
                hide-details
                label="Service Watcher"
                v-model="watcher"
                :items="watchers"
              ></v-select>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                hide-details
                placeholder="미입력시 자동 부여"
                v-model="name"
                autocomplete="off"
                label="Name"
              ></v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                hide-details
                placeholder="미입력시 자동 부여"
                v-model="port"
                autocomplete="off"
                label="Port"
                type="number"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-container>
      <v-system-bar window height="36" color="rgb(42,42,42)">
        <v-icon small>mdi-information-outline</v-icon>
        <span>Add Inference App Info</span>
        <v-spacer></v-spacer>
        <v-icon color="success" @click="addInferenceApp">mdi-check-bold</v-icon>
        <v-icon color="red" @click="closeDialog">mdi-close-thick</v-icon>
      </v-system-bar>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import Vue from 'vue';
import { Http } from '../../api/Http';
import EventBus from '../../EventBus';
export default Vue.extend<any, any, any, any>({
  data() {
    return {
      showDialog: false,
      name: null,
      port: null,
      watchers: [],
      watcher: null,
    };
  },
  created() {
    EventBus.$on('toggleInference', (payload: any) => {
      console.log(payload);
      this.watchers = payload.map((watcher: any) => {
        return {
          text: watcher.ip,
          value: watcher,
        };
      });
      if (payload && payload.length) this.watcher = payload[0];
      this.showDialog = !this.showDialog;
    });
  },
  methods: {
    async addInferenceApp() {
      var result = await Http.post(
        `http://localhost:${this.$router.currentRoute.query.port}/watcher`,
        {
          watcher: this.watcher,
          name: this.name,
          port: this.port,
        },
      );
      if (result) {
        EventBus.$emit('updateInferenceApp', {});
        this.closeDialog();
      } else {
        alert('중복된 이름이 존재합니다.');
      }
    },
    closeDialog() {
      this.showDialog = false;
    },
  },
});
</script>

<style lang="sass" scoped></style>
