import Box from '@mui/material/Box';
import { styled } from '@mui/material/styles';
import { CSSProperties } from 'react';
import { ReactSVG } from 'react-svg';

const SvgBox = styled(Box)(() => ({
  "div": {
    display: 'flex',
    alignContent: "center",
    justifyContent: "center",
  }
})) as typeof Box;

type PropsType = {
  src: string;
  fallback: React.ElementType;
  loading: React.ElementType;
  style?: CSSProperties;
  className?: string;
  beforeInjection?: (svg: SVGSVGElement) => void;
  httpRequestWithCredentials?: boolean;
};

const InjectSVG = (props: PropsType) => <SvgBox><ReactSVG {...props} /></SvgBox>

export default InjectSVG;