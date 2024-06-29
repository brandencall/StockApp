
export const setSessionStorageItem = (itemKey: string, item: any) => {

    try {
        sessionStorage.setItem(itemKey, JSON.stringify(item))
    } catch (e) {

        if (e instanceof Error) {
            if (e.name === 'QUOTA_EXEEDED_ERR') {
                console.error('Out of memory')
            }
        }
    }
    


}