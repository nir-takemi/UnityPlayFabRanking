# UnityPlayFabRanking
Unity上で、PlayFabのランキング部分を簡易的に利用するためのパッケージ。

# 対象ビルド
- iOS
- Android
- WebGL


# 準備
## PlayFab公式のドキュメントに沿って使用可能にしてください
1. [PlayFabのページ](https://azure.microsoft.com/ja-jp/services/playfab/)よりアカウントを作成
2. [クイックスタート](https://docs.microsoft.com/ja-jp/gaming/playfab/sdks/unity3d/quickstart)によりプロジェクトのセットアップを実施

## ランキングをPlayFab上で作成
1. https://developer.playfab.com/ja-JP/r/t/プロジェクトID/leaderboards/new を開く
2. 任意の値で作成
![image](https://user-images.githubusercontent.com/10418442/69739528-34a37a80-117b-11ea-859d-0a1972d25e43.png)
3. 設定画面のAPI機能 https://developer.playfab.com/ja-JP/r/t/プロジェクトID/settings/api-features にて、「クライアントにプレイヤー統計情報のポストを許可する」にチェックをする
![image](https://user-images.githubusercontent.com/10418442/69741897-30795c00-117f-11ea-98aa-eeebcca4ac73.png)

## unitypackageのDL
以下から最新のものをダウンロードしてください
https://github.com/nir-takemi/UnityPlayFabRanking/releases


# 実装
1. DLした.unitypackageをmenuからimport
![image](https://user-images.githubusercontent.com/10418442/68995076-22992080-08cd-11ea-8c88-e435b6d40dd4.png)

2. Sampleは任意で、その他にチェックがされていることを確認の上import
![image](https://user-images.githubusercontent.com/10418442/69736853-a5946380-1176-11ea-8e29-9579f0093c94.png)

3. ログインのためのprefab(ylib > UnityPlayFabCommon > Resources > Prefabs > GO_UnityPlayFab)をランキング表示/更新したいscene上に置く
　（ランキングの処理が複数sceneに存在する場合は、DontDestroyOnLoadなオブジェクト配下に置く）
![image](https://user-images.githubusercontent.com/10418442/69738478-84814200-1179-11ea-96ce-392159354fb0.png)

4. ランキング表示/更新のためのprefab(ylib > UnityPlayFabRanking > Resources > Prefabs > GO_RankingView)をランキング表示/更新したいscene上に置く
 （uGUIなので、見た目変えたい方は修正してprefabを作り直した後に配置してください）
![image](https://user-images.githubusercontent.com/10418442/69739042-5e0fd680-117a-11ea-8f36-eb13722542db.png)

5. 4で配置したRanking prefabの設定値を任意のものに入力する
![image](https://user-images.githubusercontent.com/10418442/69739175-96171980-117a-11ea-912e-1847304de5b5.png)
   1. LoadOnAwake：Awakeが実行されるタイミングで設定値に基づくランキングデータをloadするかどうかを設定
   2. RankingViewOnly：ランキングのみの表示にするかどうかを設定（基本形は名前更新、ランキングのセットで表示するprefabとなっている）
   3. RankingName：PlayFab上で作成したランキングの統計情報名を入力
   4. RankingDataSize：ランキング表示数
   5. FormatScore：スコア表示のフォーマット（カンマ表示したい場合は「{0:#,0}」などで指定）
   6. PrefabRankingData：基本はデフォルトのままで問題ないですが、独自のUIを指定したい場合は指定し直してください
   7. EventOnRankingLoad：ランキングロード後に実行されるイベント
   8. EventOnClose：RankingViewが閉じられる際に実行されるイベント

5. Unity/コード上で以下のように処理を書く
```
// Inspector上でアタッチする
[SerializeField]
private ylib.Services.UI.RankingView rankingView = null;

// スコア更新
rankingView.UpdateScore(12345);

// 可視化するだけ（そのため初回load時はちょっと時間かかる）
rankingView.gameObject.SetActive(true);
```

6. 表示例
![image](https://user-images.githubusercontent.com/10418442/69740957-8e0ca900-117d-11ea-8a7e-f91d2068157d.png)

# その他
- デフォルトの設定だとプレイヤー名が重複がされるとエラーになる
  - 設定の一般https://developer.playfab.com/ja-JP/r/t/プロジェクトID/settings/general より、「一意でないプレイヤー表示名を許可する」をチェックすれば重複は可能になります（ただし、一度変更すると以降変更できないため注意です）  
  ![image](https://user-images.githubusercontent.com/10418442/69741068-c4e2bf00-117d-11ea-8ea0-d10ab1d5d239.png)
