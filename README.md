# Bing壁纸更换器V0.1

前几天想找个软件给电脑弄个随机壁纸，偶然看到了个收费的软件，就想着既然有要花钱的还不如自己写一个练练，所以就写了这么个小玩意。

现在版本号是 `V0.1` 但是还有一些功能没写完，以后会根据需要添加更多的功能。

bing随机壁纸API：https://bing.ioliu.cn/v1/rand?w=1920&h=1080

他的git仓库：https://github.com/xCss/bing

bing每日壁纸的API：https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1

这个API拿到以后自己解析XML，加上 `https://www.bing.com` 就能用了，分辨率就改后缀前面的两个数就行。
