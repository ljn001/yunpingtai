<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="CloudPlatform.Main" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智能制造云平台</title>
    <link rel="shortcut icon" href="img/ico.ico" type="image/x-icon" />
    <link href="./res/css/index.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="mainPanel" runat="server" />
        <f:Panel ID="mainPanel" Layout="Region" CssClass="mainpanel" ShowBorder="false" ShowHeader="false" runat="server">
            <Items>
                <f:Panel ID="sidebarRegion" CssClass="sidebarregion bgpanel" RegionPosition="Left" ShowBorder="false" Width="260" ShowHeader="false" EnableCollapse="false" Collapsed="false" Layout="VBox" runat="server"
                    RegionSplit="true" RegionSplitIcon="false" RegionSplitWidth="3" RegionSplitTransparent="true">
                    <Items>
                        <f:ContentPanel CssClass="topregion" ShowBorder="false" ShowHeader="false" runat="server">
                            <div id="sideheader" class="f-widget-header f-mainheader">
                                <a class="logo" title="智能制造云平台" id="logoTitle" runat="server">智能制造云平台</a>
                            </div>
                        </f:ContentPanel>
                        <f:Panel ID="Panel1" CssClass="leftregion" BoxFlex="1" ShowBorder="true" ShowHeader="false" Layout="Fit" runat="server">
                            <Items>
                                <f:Tree runat="server" ShowBorder="false" ShowHeader="false" ID="treeMenu" EnableSingleClickExpand="true" />
                            </Items>
                        </f:Panel>
                    </Items>
                    <Listeners>
                        <f:Listener Event="splitdrag" Handler="onSidebarSplitDrag" />
                    </Listeners>
                </f:Panel>
                <f:Panel ID="bodyRegion" CssClass="bodyregion" RegionPosition="Center" ShowBorder="false" ShowHeader="false" Layout="VBox" runat="server">
                    <Items>
                        <f:ContentPanel ID="ContentPanel1" CssClass="topregion" ShowBorder="false" ShowHeader="false" runat="server">
                            <div id="header" class="f-widget-header f-mainheader">
                                <div class="header-left">
                                    <f:Button runat="server" ID="btnCollapseSidebar" CssClass="icononlyaction" ToolTip="折叠/展开侧边栏" IconAlign="Top" IconFont="_Fold" EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false" TabIndex="-1">
                                        <Listeners>
                                            <f:Listener Event="click" Handler="onFoldClick" />
                                        </Listeners>
                                    </f:Button>
                                    <div id="breadcrumb">
                                        <div class="breadcrumb-inner">
                                            <span class="breadcrumb-last">首页</span>
                                        </div>
                                        <div class="breadcrumb-icons">
                                            <a data-qtip="查看源代码" href="javascript:onToolSourceCodeClick();"><i class="f-icon f-iconfont f-iconfont-code"></i></a>
                                            <a data-qtip="刷新本页" href="javascript:onToolRefreshClick();"><i class="f-icon f-iconfont f-iconfont-refresh"></i></a>
                                            <a data-qtip="在新标签页中打开" href="javascript:onToolNewWindowClick();"><i class="f-icon f-iconfont f-iconfont-new-tab"></i></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="header-right">
                                    <f:Button runat="server" ID="btnUser" CssClass="userpicaction" Text="三生石上" IconUrl="~/res/images/blank_150.png" IconAlign="Left" EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false">
                                        <Menu runat="server">
                                            <f:MenuButton Text="修改密码" IconFont="_Pencil" EnablePostBack="false" runat="server">
                                                <Listeners>
                                                    <f:Listener Event="click" Handler="onChangePwdClick" />
                                                </Listeners>
                                            </f:MenuButton>
                                            <f:MenuSeparator runat="server" />
                                            <f:MenuButton Text="安全退出" ID="btnQuit" IconFont="_SignOut" ConfirmText="您确定退出系统吗？" ConfirmIcon="Question" runat="server" OnClick="btnQuit_Click" />
                                        </Menu>
                                    </f:Button>
                                </div>
                            </div>
                        </f:ContentPanel>
                        <f:TabStrip ID="mainTabStrip" CssClass="centerregion" ShowInkBar="true" BoxFlex="1" ShowBorder="true" EnableTabCloseMenu="true" runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="首页" BodyPadding="10px" AutoScroll="true" Icon="House" runat="server">
                                    <Content>
                                        <h2>欢迎使用</h2>
                                        智能制造云平台
                                    </Content>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>

        <f:Window ID="winChangePwd" Title="修改密码" Hidden="true" EnableIFrame="true" IFrameUrl="/Users/ChangePwd.aspx" ClearIFrameAfterClose="false" runat="server" IsModal="true" Width="600px" Height="250px" EnableClose="true" EnableMaximize="false" EnableResize="false" />
    </form>
    <script>
        var _menuStyle = F.cookie('MenuStyle') || 'tree';
        var SIDEBAR_WIDTH_CONSTANT = 260;
        // _sidebarWidth变量会随着用户拖动分隔条而改变
        var _sidebarWidth = SIDEBAR_WIDTH_CONSTANT;

        var treeMenuClientID = '<%= treeMenu.ClientID %>';
        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';

        var PARAMS = {
            mainPanel: '<%= mainPanel.ClientID %>',
            mainTabStrip: '<%= mainTabStrip.ClientID %>',
            treeMenu: '<%= treeMenu.ClientID %>',
            sidebarRegion: '<%= sidebarRegion.ClientID %>',
            btnCollapseSidebar: '<%= btnCollapseSidebar.ClientID %>',
            sourceUrl: '<%= PageContext.ResolveUrl("~/common/source.aspx") %>',
            dashboardUrl: '<%= PageContext.ResolveUrl("~/block/dashboard.aspx") %>',
            mainUrl: '<%= PageContext.ResolveUrl("~/common/main.aspx") %>',
            processNewWindowUrl: function (url) {
                return url.replace(/\/mobile\/\?file=/ig, '/mobile/');
            }
        };

        function onChangePwdClick(event) {
            F(winChangePwd).show();
        }

        // 页面控件初始化完毕后执行
        F.ready(function () {
            var treeMenu = F(treeMenuClientID);
            var mainTabStrip = F(mainTabStripClientID);
            if (!treeMenu) return;

            // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
            // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
            // mainTabStrip： 选项卡实例
            // options: 参数
            // options.updateHash： 切换Tab时，是否更新地址栏Hash值（默认值：true）
            // options.refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame（默认值：false）
            // options.refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame（默认值：false）
            // options.maxTabCount: 最大允许打开的选项卡数量
            // options.maxTabMessage: 超过最大允许打开选项卡数量时的提示信息
            // options.beforeNodeClick: 节点点击事件之前执行（返回false则不执行点击事件）
            // options.beforeTabAdd: 添加选项卡之前执行（返回false则不添加选项卡）
            F.initTreeTabStrip(treeMenu, mainTabStrip, {
                maxTabCount: 10,
                maxTabMessage: '请先关闭一些选项卡（最多允许打开 10 个）！'
            });
        });

        // 点击折叠/展开按钮
        function onFoldClick(event) {
            toggleSidebar();
        }

        // 设置折叠按钮的状态
        function setFoldButtonStatus(collapsed) {
            var foldButton = F(PARAMS.btnCollapseSidebar);
            if (collapsed) {
                foldButton.setIconFont('f-iconfont-unfold');
            } else {
                foldButton.setIconFont('f-iconfont-fold');
            }
        }

        // 获取折叠按钮的状态
        function getFoldButtonStatus() {
            var foldButton = F(PARAMS.btnCollapseSidebar);
            return foldButton.iconFont === 'f-iconfont-unfold';
        }

        // 展开侧边栏
        function expandSidebar() {
            toggleSidebar(false);
        }

        // 折叠侧边栏
        function collapseSidebar() {
            toggleSidebar(true);
        }

        // 折叠/展开侧边栏
        function toggleSidebar(collapsed) {
            var sidebarRegion = F(PARAMS.sidebarRegion);
            var treeMenu = F(PARAMS.treeMenu);
            var logoEl = sidebarRegion.el.find('.logo');

            var currentCollapsed = getFoldButtonStatus();
            if (F.isUND(collapsed)) {
                collapsed = !currentCollapsed;
            } else {
                if (currentCollapsed === collapsed) {
                    return;
                }
            }

            F.noAnimation(function () {

                setFoldButtonStatus(collapsed);

                if (!collapsed) {
                    if (_menuStyle === 'tree') {
                        logoEl.removeClass('short').text(logoEl.attr('title'));
                        sidebarRegion.setWidth(_sidebarWidth);
                        // 启用分隔条拖动
                        sidebarRegion.setSplitDraggable(true);

                        // 禁用树微型模式
                        treeMenu.miniMode = false;
                        // 重新加载树菜单
                        treeMenu.loadData();
                    } else {
                        sidebarRegion.expand();
                    }
                } else {
                    if (_menuStyle === 'tree') {
                        logoEl.addClass('short').text('平台');
                        sidebarRegion.setWidth(60);
                        // 禁用分隔条拖动
                        sidebarRegion.setSplitDraggable(false);

                        // 启用树微型模式
                        treeMenu.miniMode = true;
                        // 重新加载树菜单
                        treeMenu.loadData();
                    } else {
                        sidebarRegion.collapse();
                    }
                }
            });
        }

        // 侧边栏分隔条拖动事件
        function onSidebarSplitDrag(event) {
            _sidebarWidth = this.width;
        }
    </script>
</body>
</html>