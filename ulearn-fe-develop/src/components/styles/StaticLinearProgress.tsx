import { experimental_sx as sx, styled } from "@mui/system";
import LinearProgress, { LinearProgressProps } from "@mui/material/LinearProgress";

const withLinearProgressVariant = <P extends LinearProgressProps>(
    Component: React.ComponentType<P>,
    variant: LinearProgressProps['variant']
) => {
    // Create a new component that wraps the Typography component with the specified "variant"
    const WithVariant: React.FC<P> = (props) => {
        const { variant: variantProp, ...rest } = props;
        return <Component variant={variant || variantProp} {...rest as P} />;
    };

    WithVariant.displayName = `WithVariant(${Component.displayName || Component.name || 'Component'})`;

    return WithVariant;
}

const LinearProgressStyled = styled(LinearProgress)(() => sx({
    height: "4px", 
    width: "100%"
})) as typeof LinearProgress;

export default withLinearProgressVariant(LinearProgressStyled, "determinate");