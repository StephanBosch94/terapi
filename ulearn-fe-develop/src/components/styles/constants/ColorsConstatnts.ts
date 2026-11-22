export const COLORS = {
  PRIMARY: 'rgb(114,100,214)',
  PRIMARY_LIGHT: 'rgba(241,240,251)',
  PRIMARY_HOVER: 'rgb(98,85,194)',
  PRIMARY_5: 'rgb(248,247,253)',
  PRIMARY_10: 'rgb(241,240,251)',
  PRIMARY_20: 'rgb(216,212,244)',
  PRIMARY_40: 'rgb(199,193,239)',
  WHITE: 'rgb(255,255,255)',
  BLACK: "rgb(36,41,51)",
  BLACK_60: `rgb(124,127,133)`,
  BLACK_40: `rgb(167,169,173)`,
  BLACK_10: `rgb(233,234,235)`,
  BLACK_5: `rgb(244,244,245)`,
  BLACK_2: `rgb(251,251,251)`,
  RED_10: `rgb(251,230,230)`,
  RED: "rgb(220,0,0)",
  RED_HOVER: "rgb(199,0,0)",
  GREEN: "rgb(29,192,68)",
  GREEN_10: "rgba(232,247,236)",
  GREEN_20: "rgb(210,242,218)",
  GREEN_HOVER: "rgba(51,170,79)",
  SHADOW_PRIMARY: 'rgb(114,100,214)',
  BACKGROUND: 'rgb(250,251,255)',
  BLUE_LIGHT: 'rgb(235,245,255)',
  BLUE: 'rgb(114,100,214)',
  YELLOW_LIGHT: 'rgb(255,248,229)',
  GREEN_LIGHT: 'rgb(228,250,230)'
} as const

type ColorsKeys = keyof typeof COLORS;

export const getColor = (color: typeof COLORS[ColorsKeys], opacity: number = 1) => {
  return `${color.replace(')', '').replace('rgb', 'rgba')},${opacity})`
}

export function RGBAToHexA(r: number, g: number, b: number, a: number): string {
  let rStr = r.toString(16);
  let gStr = g.toString(16);
  let bStr = b.toString(16);
  let aStr = Math.round(a * 255).toString(16);

  if (rStr.length == 1)
    rStr = "0" + r;
  if (gStr.length == 1)
    gStr = "0" + g;
  if (bStr.length == 1)
    bStr = "0" + b;
  if (aStr.length == 1)
    aStr = "0" + a;

  return "#" + rStr + gStr + bStr + aStr;
}

export const rgbaToHex = (value: string): string => {
  const split = value.replace('rgba(', '').replace(')', '').split(',').map((x, idx) => parseFloat(x));
  return RGBAToHexA(split[0], split[1], split[2], split[3]);
}