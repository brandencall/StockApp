import SearchBar from '../SearchBar/SearchBar';
import styles from './Home.module.css'

function Home() {
    return (
        <div className={styles.container}>
            <SearchBar />
       </div>
      
  );
}

export default Home;