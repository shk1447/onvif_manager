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
                v-else-if="header.text == 'INFERENCE'"
                style="
                width:
                display: flex;
                justify-content: center;
                align-items: center;
              "
              >
                <v-icon small color="red" @click="handleSelectModel(item)"
                  >mdi-play-network-outline</v-icon
                >
              </div>
              <div
                v-else-if="header.text == 'CLOSE'"
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
import EventBus from '../../EventBus';
import { Http, WebSocketManager, WebSocketClient } from '../../api';
export default Vue.extend<any, any, any, any>({
  data() {
    return {
      shared_id: '',
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
          text: 'INFERENCE',
          sortable: false,
        },
        {
          text: 'CLOSE',
          sortable: false,
        },
      ],
      data_rows: [],
      watchers: [],
    };
  },
  methods: {
    async handleSelectModel(item: any) {
      console.log(this.$router.currentRoute.path);
      const path = await this.$electron.showDialog(
        this.$router.currentRoute.path,
      );

      Http.post(
        `http://localhost:${this.$router.currentRoute.query.port}/edge/exec/SetModel`,
        {
          ip: item.ip,
          port: item.port,
          path: path[0],
          shared_id: this.shared_id,
        },
      ).then(async res => {
        console.log(res);
        const client = await this.manager.socket(`/edge/resp/${res}`);
        client.on('data', (data: any) => {
          console.log(data);
        });
      });
      console.log(item);
    },
    handleInference() {
      // Http.post(`http://localhost:${this.port}/edge/exec/inference`, {
      //   path: path,
      // }).then(async res => {
      //   const client = await this.manager.socket(`/edge/resp/${res}`);
      //   client.on('data', (data: any) => {
      //     console.log(data);
      //   });
      // });
    },
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
      this.data_rows.map((app: any) => {
        Http.post(
          `http://localhost:${this.$router.currentRoute.query.port}/edge/exec/ConnectEngine`,
          {
            ip: app.ip,
            port: app.port,
          },
          {},
          {
            uuid: 'test',
          },
        ).then(async res => {
          console.log(res);
          const client = await this.manager.socket(`/edge/resp/${res}`);
          client.on('data', (data: any) => {
            console.log(data);
          });
          this.shared_id = res;
          (window as any).shared_id = res;
        });
      });
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
