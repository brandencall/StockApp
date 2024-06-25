
export const abbreviateNumber = (num: number): string => {
    if (num >= 1_000_000_000_000 ||  num <= -1_000_000_000_000) {
        return "$" + (num / 1_000_000_000_000).toFixed(1).replace(/\.0$/, '') + 'T';
    } else if (num >= 1_000_000_000 || num <= -1_000_000_000) {
        return "$" + (num / 1_000_000_000).toFixed(1).replace(/\.0$/, '') + 'B';
    } else if (num >= 1_000_000 || num <= -1_000_000) {
        return "$" + (num / 1_000_000).toFixed(1).replace(/\.0$/, '') + 'M';
    } else if (num >= 1_000 || num <= -1_000) {
        return "$" + (num / 1_000).toFixed(1).replace(/\.0$/, '') + 'K';
    } else {
        return num.toString();
    }
};

export const formatNumberWithCommas = (num: number): string => {

    num = num / 1000;

    return "$" + num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
};