# VsNerdX (2.4.0)
[NERDTree](https://github.com/scrooloose/nerdtree) inspired Solution Explorer for Visual Studio. It integrates VIM bindings for tree navigation and manipulation into Visual Studio's hierarchy windows.

# Install
Follow the instructions on **[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=mstevius.vs-nerdx-solution-explorer)** or use **Tools / Extensions and Updates**

If building from source, use the resulting `VsNerdX.vsix`.

Currently supported Visual Studio versions are 2017, 2019, and 2022. 

# Usage
#### Directory node mappings
* `o` - open & close node
* `O` - recursively open node
* `x` - close parent of node
* `X` - close all child nodes  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;of current node recursively

#### File node mappings
* `<Enter>` - open file
* `go` - preview file
* `h` - open split
* `v` - open vertical split

#### Tree navigation mappings
* `P` - go to parent
* `t` - go to next sibling
* `T` - go to last child
* `s` - go to prev sibling
* `S` - go to first child
* `gg` - go to top
* `G` - go to bottom

#### Node editing mappings
* `DD` - delete 
* `dd` - cut 
* `yy` - copy 
* `yp` - copy full path
* `yw` - copy visible text
* `p` - paste 
* `l` - rename

#### Tree filtering mappings
* `I` - toggle show all files 

#### Other mappings
* `/` - enter find mode - stops all processing of keys with the exception of Esc
* `Esc` - exit find mode - resumes handling of navigation keys
* `?` - toggle help

# Providing Feedback
* File a bug or request a new feature in [issue tracker](https://github.com/mstevius/vs-nerdx/issues).
* [Tweet](https://twitter.com/stevium) me  with any other feedback.
