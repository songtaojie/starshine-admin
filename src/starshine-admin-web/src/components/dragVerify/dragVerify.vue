<template>
	<div ref="dragVerify" class="drag_verify" :style="dragVerifyStyle" @mousemove="dragMoving" @mouseup="dragMouseFinish"
		@mouseleave="dragMouseFinish" @touchmove.prevent="dragTouching" @touchend.prevent="dragTouchFinish">

		<div class="dv_progress_bar" :class="{goFirst2:isOk}" ref="progressBar" :style="progressBarStyle">

		</div>
		<div class="dv_text" :style="textStyle" ref="message">
			<slot name="textBefore" v-if="$slots.textBefore"></slot>
			{{message}}
			<slot name="textAfter" v-if="$slots.textAfter"></slot>
		</div>

		<div class="dv_handler dv_handler_bg" :class="{goFirst:isOk}" @mousedown="dragMoveStart" @touchstart.prevent="dragTouchStart"
			ref="handler" :style="handlerStyle">
			<i :class="handlerIcon"></i>
		</div>

	</div>
</template>

<script lang="ts">
export default {
	name: "dragVerify",
	props: {
		isPassing: {
			type: Boolean,
			default: false
		},
		width: {
			type: Number,
			default: 250
		},
		height: {
			type: Number,
			default: 40
		},
		text: {
			type: String,
			default: "swiping to the right side"
		},
		successText: {
			type: String,
			default: "success"
		},
		background: {
			type: String,
			default: "#eee"
		},
		progressBarBg: {
			type: String,
			default: "#76c61d"
		},
		completedBg: {
			type: String,
			default: "#76c61d"
		},
		circle: {
			type: Boolean,
			default: false
		},
		radius: {
			type: String,
			default: "4px"
		},
		handlerIcon: {
			type: String
		},
		successIcon: {
			type: String
		},
		handlerBg: {
			type: String,
			default: "#fff"
		},
		textSize: {
			type: String,
			default: "14px"
		},
		textColor: {
			type: String,
			default: "#333"
		}
	},
	mounted: function () {
		const dragEl = this.$refs.dragVerify  as HTMLElement;
		dragEl.style.setProperty("--textColor", this.textColor);
		dragEl.style.setProperty("--width", Math.floor(this.width / 2) + "px");
		dragEl.style.setProperty("--pwidth", -Math.floor(this.width / 2) + "px");
		console.log(this.$slots);
	},
	computed: {
		handlerStyle: function () {
			return {
				width: this.height + "px",
				height: this.height + "px",
				background: this.handlerBg
			};
		},
		message: function () {
			return this.isPassing ? this.successText : this.text;
		},
		dragVerifyStyle: function () {
			return {
				width: this.width + "px",
				height: this.height + "px",
				lineHeight: this.height + "px",
				background: this.background,
				borderRadius: this.circle ? this.height / 2 + "px" : this.radius
			};
		},
		progressBarStyle: function () {
			return {
				background: this.progressBarBg,
				height: this.height + "px",
				borderRadius: this.circle
					? this.height / 2 + "px 0 0 " + this.height / 2 + "px"
					: this.radius
			};
		},
		textStyle: function () {
			return {
				height: this.height + "px",
				width: this.width + "px",
				fontSize: this.textSize
			};
		}
	},
	data() {
		return {
			isMoving: false,
			x: 0,
			isOk: false,
			internalState: {
				isPassing:this.isPassing,
				width:this.width,
				height:this.height,
				text:this.text,
				successText:this.successText,
				background:this.background,
				progressBarBg:this.progressBarBg,
				completedBg:this.completedBg,
				circle:this.circle,
				radius:this.radius,
				handlerIcon:this.handlerIcon,
				successIcon:this.successIcon,
				handlerBg:this.handlerBg,
				textSize:this.textSize,
				textColor:this.textColor
			}
		};
	},
	methods: {
		dragMoveStart(e:MouseEvent) {
			if (!this.isPassing) {
				this.isMoving = true;
				this.x = e.pageX
			}
			this.$emit("handlerMove");
		},
		dragTouchStart(e:TouchEvent) {
			if (!this.isPassing) {
				this.isMoving = true;
				this.x = e.touches[0].pageX
			}
			this.$emit("handlerMove");
		},
		dragMovingExec:function(_x:number){
			var handler = this.$refs.handler as HTMLElement;
			var progressBarRef = this.$refs.progressBar as HTMLElement;
			if (_x > 0 && _x <= this.width - this.height) {
				handler.style.left = _x + "px";
				progressBarRef.style.width = _x + this.height / 2 + "px";
			} else if (_x > this.width - this.height) {
				handler.style.left = this.width - this.height + "px";
				progressBarRef.style.width = this.width - this.height / 2 + "px";
				this.passVerify();
			}
		},
		dragMoving: function (e:MouseEvent) {
			if (this.isMoving && !this.isPassing) {
				var _x = e.pageX - this.x;
				this.dragMovingExec(_x)
			}
		},
		dragTouching: function (e:TouchEvent) {
			if (this.isMoving && !this.isPassing) {
				var _x = e.touches[0].pageX - this.x;
				this.dragMovingExec(_x)
			}
		},
		dragFinish:function(_x:number) {
			if (_x < this.width - this.height) {
				this.isOk = true;
				var that = this;
				var handlerRef = that.$refs.handler as HTMLElement;
				var progressBarRef = that.$refs.progressBar as HTMLElement;
				setTimeout(function () {
					handlerRef.style.left = "0";
					progressBarRef.style.width = "0";
					that.isOk = false;
				}, 500);
				this.$emit("passfail");
			} else {
				var handlerRef = this.$refs.handler as HTMLElement;
				var progressBarRef = this.$refs.progressBar as HTMLElement;
				handlerRef.style.left = this.width - this.height + "px";
				progressBarRef.style.width =
					this.width - this.height / 2 + "px";
				this.passVerify();
			}
			this.isMoving = false;
		},
		dragMouseFinish: function (e:MouseEvent) {
			if (this.isMoving && !this.isPassing) {
				var _x = e.pageX - this.x;
				this.dragFinish(_x)
			}
		},
		dragTouchFinish: function (e:TouchEvent) {
			if (this.isMoving && !this.isPassing) {
				var _x = e.changedTouches[0].pageX - this.x;
				this.dragFinish(_x)
			}
		},
		passVerify: function () {
			this.$emit("update:isPassing", true);
			this.isMoving = false;
			var handlerRef = this.$refs.handler as HTMLElement;
			if(this.successIcon !== undefined)
				handlerRef.children[0].className = this.successIcon;
			var progressBarRef = this.$refs.progressBar as HTMLElement;
			progressBarRef.style.background = this.completedBg;
			var messageRef = this.$refs.message as HTMLElement;
			messageRef.style["-webkit-text-fill-color"] = "unset";
			messageRef.style.animation = "slidetounlock2 3s infinite";
			messageRef.style.color = "#fff";
			this.$emit("passcallback");
		},
		reset: function () {
			if(this.$options.data) {
				const oriData = this.internalState;
				for (const key in oriData) {
					if (Object.prototype.hasOwnProperty.call(oriData, key)) {
						this[key] = oriData[key]
					}
				}
			}
			
			var handlerRef = this.$refs.handler as HTMLElement;
			var messageRef = this.$refs.message as HTMLElement;
			var progressBarRef = this.$refs.progressBar as HTMLElement;
			handlerRef.style.left = "0";
			progressBarRef.style.width = "0";
			if(this.handlerIcon !== undefined)
				handlerRef.children[0].className = this.handlerIcon;
			messageRef.style["-webkit-text-fill-color"] = "transparent";
			messageRef.style.animation = "slidetounlock 3s infinite";
			messageRef.style.color = this.background;
		}
	}
};
</script>
<style scoped>
.drag_verify {
	position: relative;
	background-color: #e8e8e8;
	text-align: center;
	overflow: hidden;
}

.drag_verify .dv_handler {
	position: absolute;
	top: 0px;
	left: 0px;
	cursor: move;
}

.drag_verify .dv_handler i {
	color: #666;
	padding-left: 0;
	font-size: 16px;
}

.drag_verify .dv_handler .el-icon-circle-check {
	color: #6c6;
	margin-top: 9px;
}

.drag_verify .dv_progress_bar {
	position: absolute;
	height: 34px;
	width: 0px;
}

.drag_verify .dv_text {
	position: absolute;
	top: 0px;
	color: transparent;
	-moz-user-select: none;
	-webkit-user-select: none;
	user-select: none;
	-o-user-select: none;
	-ms-user-select: none;
	background: -webkit-gradient(linear,
			left top,
			right top,
			color-stop(0, var(--textColor)),
			color-stop(0.4, var(--textColor)),
			color-stop(0.5, #fff),
			color-stop(0.6, var(--textColor)),
			color-stop(1, var(--textColor)));
	-webkit-background-clip: text;
	-webkit-text-fill-color: transparent;
	-webkit-text-size-adjust: none;
	animation: slidetounlock 3s infinite;
}

.drag_verify .dv_text * {
	-webkit-text-fill-color: var(--textColor);
}

.goFirst {
	left: 0px !important;
	transition: left 0.5s;
}

.goFirst2 {
	width: 0px !important;
	transition: width 0.5s;
}
</style>
<style>
@-webkit-keyframes slidetounlock {
	0% {
		background-position: var(--pwidth) 0;
	}

	100% {
		background-position: var(--width) 0;
	}
}

@-webkit-keyframes slidetounlock2 {
	0% {
		background-position: var(--pwidth) 0;
	}

	100% {
		background-position: var(--pwidth) 0;
	}
}
</style>