window.pickLogFile = () => {
    return new Promise((resolve) => {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = '.log';
        input.multiple = true;
        input.onchange = async () => {
            const files = Array.from(input.files);
            const results = [];

            for (const file of files) {
                const text = await file.text();
                results.push({
                    name: file.name,
                    content: text
                });
            }

            resolve(results);
        };

        input.click();
    });
};
