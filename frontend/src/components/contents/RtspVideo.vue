<template>
  <video controls autoplay class="rtsp-video" ref="player" />
</template>

<script lang="ts">
import Vue from 'vue';
import flvjs from 'flv.js';
import { Http, WebSocketManager, WebSocketClient } from '../../api';
interface IRtspVideo {
  name: string;
  port: string;
  hostname: string;
  username: string;
  password: string;
  type: string;
  stream_url: string;
}

export default Vue.extend<IRtspVideo, any, any, any>({
  created() {
    this.manager = new WebSocketManager('127.0.0.1', this.port);
  },
  mounted() {
    Http.post(`http://localhost:${this.port}/edge/exec/StartInference`, {
      ip: '170.101.20.126',
      port: 5001,
      rtsp: {
        url: this.stream_url,
        username: this.username,
        password: this.password,
      },
    }).then(async res => {
      const client = await this.manager.socket(`/edge/resp/${res}`);
      client.on('data', (data: any) => {
        console.log(data);
      });
    });
    if (flvjs.isSupported()) {
      let video = this.$refs.player;
      if (video) {
        this.player = flvjs.createPlayer({
          type: 'flv',
          isLive: true,
          url: `ws://localhost:${this.port}/rtsp/stream/${this.name}`,
        });
        this.player.attachMediaElement(video);
        try {
          this.player.load();
          this.player.play();
        } catch (error) {
          console.log(error);
        }
      }
    }
  },
  methods: {
    destroyPlayer() {
      this.player.destroy();
    },
  },
  beforeDestroy() {
    // this.$electron.send('inspect/stop', {
    //   name: this.name,
    //   stream_url: this.stream_url,
    //   username: this.username,
    //   password: this.password,
    // });
  },
});
</script>

<style scoped>
.rtsp-video {
  width: 100%;
  height: 100%;
}
</style>
