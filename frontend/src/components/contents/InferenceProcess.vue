<template>
  <div style="width: 100%; height: 100%" ref="data_table">
    <v-system-bar window height="36" color="rgb(42,42,42)">
      <v-icon small>mdi-information-outline</v-icon>
      <span>you can start/stop/manage process.</span>
      <v-spacer></v-spacer>
      <v-icon color="success" small @click="handleShowDialog"
        >mdi-plus-thick</v-icon
      >
      <v-icon color="success" small @click="handleDiscovery"
        >mdi-refresh</v-icon
      >
    </v-system-bar>
    <v-data-table
      v-if="headers.length > 0"
      fixed-header
      :height="'100%'"
      class="data-table"
      :headers="headers"
      :items="data_rows"
      dense
      hide-default-footer
      :items-per-page="-1"
    >
      <template v-slot:body="{ items }">
        <tbody>
          <tr v-for="(item, idx) in items" :key="idx" @click="() => {}">
            <td v-for="header in headers" :key="header.value">
              <div
                v-if="header.value"
                :style="
                  header.disabled ? 'pointer-events:none;opacity:.5;' : ''
                "
              >
                <span>{{ item[header.value] }}</span>
              </div>
              <div
                v-else
                style="
                width:
                display: flex;
                justify-content: center;
                align-items: center;
              "
              >
                <v-icon small color="orange" @click="() => {}"
                  >mdi-pencil</v-icon
                >
                <v-icon small color="red" @click="() => {}"
                  >mdi-close-thick</v-icon
                >
              </div>
            </td>
          </tr>
        </tbody>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { WebSocketClient } from '../../api/WebSocket';
import EventBus from '../../EventBus';
export default Vue.extend<any, any, any, any>({
  data() {
    return {
      headers: [
        {
          value: 'pid',
          text: 'PID',
        },
        {
          type: 'text',
          value: 'name',
          text: 'NAME',
        },
        {
          type: 'text',
          value: 'ip',
          text: 'IP',
        },
        {
          type: 'text',
          value: 'port',
          text: 'PORT',
        },
        {
          type: 'text',
          value: 'status',
          text: 'STATUS',
        },
        {
          text: 'ACTION',
          sortable: false,
        },
      ],
      data_rows: [],
      watchers: [],
    };
  },
  methods: {
    handleShowDialog() {
      EventBus.$emit('toggleInference', this.watchers);
    },
    handleDiscovery() {
      console.log('discovery');
      this.client.ws.send('update');
    },
    updateWatcher(watchers: any) {
      console.log('update');
      this.watchers = watchers;
    },
    updateApps(apps: any) {
      this.data_rows = apps;
    },
    handleSocketState(state: boolean, client: WebSocketClient) {
      if (state) {
        console.log('discovery');
        this.client = client;
        client.on('data', (data: any) => {
          this.updateWatcher(data.watchers);
          this.updateApps(data.apps);
        });
        client.ws.send('update');
      }
    },
  },
  created() {
    EventBus.$on('updateInferenceApp', this.handleDiscovery);
    new WebSocketClient(
      `ws://localhost:${this.$router.currentRoute.query.port}/watcher/discovery`,
      this.handleSocketState,
    );
  },
  beforeDestroy() {
    this.client.close();
  },
});
</script>

<style scoped>
.data-table {
  height: calc(100% - 36px);
}
</style>
