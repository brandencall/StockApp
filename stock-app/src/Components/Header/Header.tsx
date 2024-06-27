import styles from './Header.module.css'


function Header() {
  return (
      <header className={styles.header_main}>
          <a href="/">Home</a>
          <a href="/About">About</a>
      </header>
  );
}

export default Header;