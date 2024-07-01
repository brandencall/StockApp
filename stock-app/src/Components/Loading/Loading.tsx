import ReactLoading, { LoadingType } from 'react-loading';

interface LoadingProps {
    type: LoadingType,
    color: string,
    height: string | number,
    width: string | number
}

function Loading({ type, color, height, width }: LoadingProps) {


    return (
        <ReactLoading type={type} color={color} height={height} width={width} />
  );
}

export default Loading;