# VsNerdX (2.2.0)
[NERDTree](https://github.com/scrooloose/nerdtree) inspired Solution Explorer for Visual Studio. It integrates Vim-like bindings for tree navigation and manipulation into Visual Studio's hierarchy windows.

# Installation
Simplest way to install is through the **Extension Manager** or the **[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=mstevius.vs-nerdx-solution-explorer)**.
Or if building from source, use the resulting `VsNerdX.vsix`.

Currently supported Visual Studio version is 2017.

# QuickHelp
#### Directory node mappings
* `o` - open & close node
* `O` - recursively open node
* `x` - close parent of node
* `X` - close all child nodes  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;of current node recursively

#### File node mappings
* `<Enter>` - open file
* `go` - preview file
* `i` - open split
* `s` - open vertical split

#### Tree navigation mappings
* `P` - go to parent
* `j` - go to next sibling
* `J` - go to last child
* `k` - go to prev sibling
* `K` - go to first child
* `gg` - go to top
* `G` - go to bottom

#### Node editing mappings
* `dd` - delete 
* `cc` - cut 
* `yy` - copy 
* `yp` - copy full path
* `yw` - copy visible text
* `p` - paste 
* `r` - rename

#### Tree filtering mappings
* `I` - toggle show all files 

#### Other mappings
* `/` - enter find mode - stops all processing of keys with the exception of Esc
* `Esc` - exit find mode - resumes handling of navigation keys
* `?` - toggle help

# Providing Feedback
* File a bug or request a new feature in [issue tracker](https://github.com/mstevius/vs-nerdx/issues).
* [Tweet](https://twitter.com/stevium) me  with any other feedback.
