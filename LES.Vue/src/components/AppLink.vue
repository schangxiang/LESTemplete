<template>
  <component :is="type"
             v-bind="linkProps(to)">
    <slot />
  </component>
</template>

<script>
import { isExternal } from '../../util/validate'

export default {
  props: {
    to: {
      type: String,
      required: true
    }
  },
  computed: {
    isExternal () {
      return isExternal(this.to)
    },
    type () {
      if (this.isExternal) {
        return 'a'
      }
      return 'router-link'
    }
  },
  methods: {
    linkProps (to) {
      if (this.isExternal) {
        return {
          href: to,
          target: '_blank',
          style: 'color:#fff;'
        }
      }
      return {
        to: to
      }
    }
  }
}
</script>
