<template>
  <div style="height: 100%; width: 100%; background: #222222">
    <div class="expand-title" style="border-top: 1px solid rgb(51, 51, 51)">
      <div style="font-size: 0.7rem">
        {{ name }}
      </div>
      <v-spacer></v-spacer>
      <v-hover v-slot:default="{ hover }">
        <div style="padding: 0em 0em 0em 0.5em; cursor: pointer">
          <v-icon
            @click="handleDiscovery"
            small
            :color="hover ? 'rgb(200,200,200)' : 'rgb(133, 133, 133)'"
            >mdi-refresh</v-icon
          >
        </div>
      </v-hover>
      <v-menu offset-y>
        <template v-slot:activator="{ on }">
          <v-hover v-slot:default="{ hover }">
            <div v-on="on" style="padding: 0em 0em 0em 0.5em; cursor: pointer">
              <v-icon
                small
                :color="hover ? 'rgb(200,200,200)' : 'rgb(133, 133, 133)'"
                >mdi-plus</v-icon
              >
            </div>
          </v-hover>
        </template>
        <v-list dense>
          <v-list-item-group>
            <v-list-item
              v-for="(item, i) in items"
              :key="i"
              @click="createContent(item)"
            >
              <v-list-item-icon>
                <v-icon
                  small
                  v-text="item.icon"
                  :color="'hsla(0,0%,100%,.7)'"
                ></v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title
                  v-text="item.text"
                  style="color: hsla(0, 0%, 100%, 0.7)"
                ></v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-menu>
    </div>
    <div
      :class="expand ? 'expand-content expand' : 'expand-content'"
      ref="ref_obj_list"
    >
      <v-treeview
        ref="tree"
        :items="children"
        item-key="id"
        item-text="name"
        class="content-tree-view"
        return-object
        dense
        open-all
        hoverable
        v-if="children.length"
      >
        <template v-slot:prepend="{ item }">
          <v-icon small color="hsla(0, 0%, 100%, 0.85)" v-if="item.children">
            {{ 'mdi-folder' }}
          </v-icon>
          <v-icon
            style="font-size: 20px"
            color="hsla(0, 0%, 100%, 0.85)"
            v-else-if="item.type === 'rtsp'"
          >
            {{ 'mdi-video-outline' }}
          </v-icon>
        </template>
        <template v-slot:label="{ item }">
          <div
            tabindex="0"
            style="
              outline: none;
              width: 100%;
              height: 100%;
              cursor: pointer;
              color: hsla(0, 0%, 100%, 0.85);
              font-size: 0.8rem;
              user-select: none;
            "
            @dblclick="onLoadContent(item)"
          >
            {{ item.name }}
          </div>
        </template>

        <template v-slot:append="{ item }">
          <v-tooltip right v-if="!item.children">
            <template v-slot:activator="{ on }">
              <v-icon
                color="hsla(0, 0%, 100%, 0.85)"
                @click="handleDetect(item)"
                v-on="on"
                small
                :class="item.detect ? 'detecting' : ''"
                style="margin-right: 4px"
                >mdi-record-circle-outline</v-icon
              >
            </template>
            <span>Detect</span>
          </v-tooltip>

          <!-- <v-tooltip right v-if="item.id != 'root'">
            <template v-slot:activator="{ on }">
              <v-icon
                color="hsla(0, 0%, 100%, 0.85)"
                @click="removeContent(item)"
                v-on="on"
                small
                >mdi-delete</v-icon
              >
            </template>
            <span>Delete</span>
          </v-tooltip> -->
        </template>
      </v-treeview>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import EventBus from '../EventBus';
import { Http, WebSocketManager, WebSocketClient } from '../api';

export default Vue.extend<any, any, any, any>({
  data() {
    return {
      manager: new WebSocketManager(
        '127.0.0.1',
        this.$router.currentRoute.query.port,
      ),
      expand: true,
      name: 'Resources',
      items: [
        {
          icon: 'mdi-video',
          text: 'RTSP',
        },
        {
          icon: 'mdi-database',
          text: 'NVR',
        },
      ],
      children: [],
    };
  },
  methods: {
    async handleDiscovery() {
      const client: WebSocketClient = await this.manager.socket(
        '/rtsp/discovery',
      );
      client.ws.send('update');
    },
    async createContent(item: any) {
      console.log(item);
    },
    async onLoadContent(item: any) {
      EventBus.$emit('addContent', item);
    },
    updateResource(data: any) {
      var rtspVideos = data.map((item: any) => {
        item.record = false;
        item.detect = false;
        return item;
      }) as any;
      this.children = [
        {
          id: 'root01',
          name: 'Rtsp Video (' + rtspVideos.length + ')',
          children: rtspVideos,
        },
        {
          id: 'root02',
          name: 'Record Video',
          children: [] as any,
        },
      ] as any;
    },
    async handleRecord(item: any) {
      item.record = !item.record;
      const client = await this.manager.socket(`/rtsp/record/${item.name}`);
      if (item.record) {
        client.ws.send('start');
      } else {
        client.ws.send('stop');
      }
    },
    async handleDetect(item: any) {
      if (item.detect) {
        Http.post(
          `http://localhost:${this.$router.currentRoute.query.port}/edge/exec/ThreadExit`,
          {},
          {},
          { uuid: item.thread_id },
        ).then(async res => {
          const client = await this.manager.socket(`/edge/resp/${res}`);
          client.on('data', (data: any) => {
            console.log(data);
          });
          item.detect = !item.detect;
        });
      } else {
        Http.post(
          `http://localhost:${this.$router.currentRoute.query.port}/edge/exec/StartInference`,
          {
            ip: '170.101.20.126',
            port: 5001,
            rtsp: {
              url: item.stream_url,
              username: item.username,
              password: item.password,
            },
            shared_id: 'test',
          },
        ).then(async res => {
          const client = await this.manager.socket(`/edge/resp/${res}`);
          client.on('data', (data: any) => {
            data.map((item: any) => {
              console.log(item.IsNormal);
            });
          });
          item.detect = !item.detect;
          item.thread_id = res;
        });
      }
    },
  },
  created() {
    this.manager.socket('/rtsp/discovery').then((client: WebSocketClient) => {
      client.on('data', this.updateResource);
      client.ws.send('update');
    });
  },
  mounted() {
    // this.handleDiscovery();
  },
  async beforeDestroy() {
    this.manager.dispose();
  },
});
</script>
<style>
.content-tree-view {
  height: 100%;
  color: hsla(0, 0%, 100%, 0.7);
}

.expand-title {
  min-height: 40px;
  width: 100%;
  /* height: 3em; */
  padding: 0.5em;
  display: flex;
  align-items: center;
  box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.1), 0 2px 2px 0 rgba(0, 0, 0, 0.05),
    0 1px 5px 0 rgba(0, 0, 0, 0.1);
}
.content-tree-view .v-treeview-node__prepend {
  display: flex;
  align-items: center;
}

.content-tree-view .v-treeview-node__append {
  display: flex;
  align-items: center;
}

.expand-content {
  height: calc(100% - 3em) !important;
  overflow: auto;
  width: 100%;
  transition: 0.3s ease all;
  min-height: 0px;
  height: 0px;
  visibility: hidden;
}
.expand-content.expand {
  min-height: 250px;
  visibility: visible;
}

.detecting {
  animation-name: blink;
  animation-duration: 1s;
  animation-iteration-count: infinite;
}

@keyframes blink {
  0% {
    color: hsla(0, 0%, 100%, 0.85);
  }
  50% {
    color: red;
  }
  100% {
    color: hsla(0, 0%, 100%, 0.85);
  }
}
</style>
