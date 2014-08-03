#tail コマンドを windows 環境で

オプションは -f と -n を実装
環境変数(path)に入れると幸せ度↑
.net framework4.0 以降で動作

#処方箋)

tail -f "C:\hoge.log" "D:\hoge.log"
tail -n 10 -f "C:*.log"
tail -f "C:*.log" | find "fuga"
