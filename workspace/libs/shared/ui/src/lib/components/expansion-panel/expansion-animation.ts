import {
  AnimationTriggerMetadata,
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';

/** ExpansionPanel's states. */
export type ExpansionPanelState = 'expanded' | 'collapsed';

export const EXPANSION_PANEL_ANIMATION_TIMING = '300ms ease-out';

export const appExpansionAnimations: {
  readonly bodyExpansion: AnimationTriggerMetadata;
} = {
  bodyExpansion: trigger('bodyExpansion', [
    state(
      'expanded',
      style({
        height: '*',
        visibility: '',
        opacity: 1,
        transform: 'translateY(0)',
      }),
    ),
    state(
      'collapsed, void',
      style({
        height: '0',
        visibility: 'hidden',
        opacity: 0,
        transform: 'translateY(-10px)',
      }),
    ),
    transition(
      'expanded <=> collapsed',
      animate(EXPANSION_PANEL_ANIMATION_TIMING),
    ),
  ]),
};
