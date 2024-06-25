import { useState } from 'react';
import styles from './ToggleSwitch.module.css';


interface ToggleSwitchProps {
    handleClick: (isOn: boolean) => void;
}

function ToggleSwitch({ handleClick }: ToggleSwitchProps) {
    const [isOn, setIsOn] = useState(true);

    const handleToggle = () => {
        setIsOn(!isOn);
        handleClick(!isOn)
    };

    return (
        <label className={styles.switch}>
            <input type="checkbox" checked={isOn} onChange={handleToggle} />
            <span className={styles.slider}></span>
        </label>
    );
}

export default ToggleSwitch;