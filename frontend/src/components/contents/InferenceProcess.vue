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
import { WebSocketClient, WebSocketManager } from '../../api/WebSocket';
import EventBus from '../../EventBus';
export default Vue.extend<any, any, any, any>({
  data() {
    return {
      manager: new WebSocketManager(
        '127.0.0.1',
        this.$router.currentRoute.query.port,
      ),
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
    async handleDiscovery() {
      const client: WebSocketClient = await this.manager.socket(
        '/watcher/discovery',
      );
      client.ws.send('update');
    },
    updates(data: any) {
      console.log('update');
      this.watchers = data.watchers;
      this.data_rows = data.apps;
    },
  },
  created() {
    this.manager
      .socket('/watcher/discovery')
      .then((client: WebSocketClient) => {
        client.on('data', this.updates);
        client.ws.send('update');
      });
    EventBus.$on('updateInferenceApp', this.handleDiscovery);
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
