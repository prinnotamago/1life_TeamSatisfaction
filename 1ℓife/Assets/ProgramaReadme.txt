命名規則
型名　GameObject　(単語単語で始まり大文字)

変数名　jampValue　(始め小文字で後の単語の始まりは大文字)

定数名　JAMP_LIMIT　(すべて大文字、単語単語の間はアンダーバー)

関数名　型名と一緒　JampUpdata　(単語単語で始まり大文字)

フォルダ名　型名と一緒

マテリアル　型名と一緒

プロパティ　定数と一緒

クラスのフィールド内での宣言は　アンダーバーをつけるように
private int _homo;
public int _HOMO{ get{ return _homo; }}
private const int _HOMO_VALUE;　
etc...

publicをつけていいのはプロパティと関数だけ(関数もできるだけprivateにしたい)
private int _homo;
public int _HOMO{ get{ return _homo; }}
public void HomoCreate()
{
	
}